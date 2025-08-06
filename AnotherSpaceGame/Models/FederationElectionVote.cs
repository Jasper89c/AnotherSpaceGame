public class FederationElectionVote
{
    public int Id { get; set; }
    public int FederationId { get; set; }
    public string? VoterId { get; set; }
    public string? CandidateId { get; set; }
    public int VoteWeight { get; set; } // Equal to voter's TotalPlanets
}