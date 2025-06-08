using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AnotherSpaceGame.Models;
using AnotherSpaceGame.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class FederationElectionModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public FederationElectionModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Federations Federation { get; set; }
        public List<ApplicationUser> Members { get; set; } = new();
        public Dictionary<string, int> CandidateVotes { get; set; } = new();
        public int TotalFederationPlanets { get; set; }
        public int VotesNeeded { get; set; }
        public string? FeedbackMessage { get; set; }

        [BindProperty(SupportsGet = true)]
        public int FederationId { get; set; }

        public async Task<IActionResult> OnGetAsync(int federationId)
        {
            FederationId = federationId;
            Federation = await _context.Federations
                .Include(f => f.FederationMembers)
                .Include(f => f.FederationLeader)
                .FirstOrDefaultAsync(f => f.Id == FederationId);

            if (Federation == null)
                return NotFound();

            Members = Federation.FederationMembers.ToList();
            TotalFederationPlanets = Members.Sum(m => m.TotalPlanets);
            VotesNeeded = TotalFederationPlanets / 2;

            // Tally votes
            CandidateVotes = _context.Set<FederationElectionVote>()
                .Where(v => v.FederationId == FederationId)
                .GroupBy(v => v.CandidateId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(v => v.VoteWeight)
                );

            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostVoteAsync(int federationId, string candidateId)
        {
            var userName = User.Identity?.Name;
            var voter = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            if (voter == null)
            {
                FeedbackMessage = "User not found.";
                return await OnGetAsync(federationId);
            }

            // Remove previous vote if exists
            var existingVote = await _context.Set<FederationElectionVote>()
                .FirstOrDefaultAsync(v => v.FederationId == federationId && v.VoterId == voter.Id);

            if (existingVote != null)
                _context.Set<FederationElectionVote>().Remove(existingVote);

            // Add new vote
            var vote = new FederationElectionVote
            {
                FederationId = federationId,
                VoterId = voter.Id,
                CandidateId = candidateId,
                VoteWeight = voter.TotalPlanets
            };
            _context.Set<FederationElectionVote>().Add(vote);
            await _context.SaveChangesAsync();

            FeedbackMessage = "Your vote has been cast.";
            return await OnGetAsync(federationId);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostBecomeLeaderAsync(int federationId)
        {
            var userName = User.Identity?.Name;
            var candidate = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            if (candidate == null)
            {
                FeedbackMessage = "User not found.";
                return await OnGetAsync(federationId);
            }

            // Tally votes for this candidate
            var votes = await _context.Set<FederationElectionVote>()
                .Where(v => v.FederationId == federationId && v.CandidateId == candidate.Id)
                .SumAsync(v => v.VoteWeight);

            var federation = await _context.Federations
                .Include(f => f.FederationLeader)
                .FirstOrDefaultAsync(f => f.Id == federationId);

            var members = await _context.Users.Where(u => u.FederationId == federationId).ToListAsync();
            var totalPlanets = members.Sum(m => m.TotalPlanets);
            var votesNeeded = totalPlanets / 2;

            if (votes < votesNeeded)
            {
                FeedbackMessage = "Not enough votes to become the new leader.";
                return await OnGetAsync(federationId);
            }

            // Update federation leader
            var oldLeader = federation.FederationLeader;
            federation.FederationLeaderId = candidate.Id;
            federation.FederationLeader = candidate;
            _context.Federations.Update(federation);

            // Optionally, update old leader's role if you have a role property

            await _context.SaveChangesAsync();

            FeedbackMessage = "You are now the new federation leader!";
            return await OnGetAsync(federationId);
        }
    }
}
