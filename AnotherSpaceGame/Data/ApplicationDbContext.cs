using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace AnotherSpaceGame.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<ViralReversedShips> ViralReversedShips { get; set; }
        public DbSet<CounterAttacks> CounterAttacks { get; set; }
        public DbSet<Missions> Missions { get; set; }
        public DbSet<UserProjects> UserProjects { get; set; }
        public DbSet<PrivateMessage> PrivateMessages { get; set; }
        public DbSet<Commodities> Commodities { get; set; }
        public DbSet<Fleet> Fleets { get; set; }
        public DbSet<Planets> Planets { get; set; }
        public DbSet<Turns> Turns { get; set; } 
        public DbSet<Artifacts> Artifacts { get; set; }
        public DbSet<Infrastructer> Infrastructers { get; set; } 
        public DbSet<EClassResearch> EClassResearches { get; set; }
        public DbSet<CyrilClassResearch> CyrilClassResearches { get; set; }
        public DbSet<StrafezResearch> StrafezResearches { get; set; }
        public DbSet<FClassResearch> FClassResearches { get; set; }
        public DbSet<ProjectsResearch> ProjectsResearches { get; set; }
        public DbSet<ViralSpecificResearch> ViralSpecificResearches { get; set; }
        public DbSet<CollectiveSpecificResearch> CollectiveSpecificResearches { get; set; }
        public DbSet<TerranResearch> TerranResearches { get; set; }
        public DbSet<AMinerResearch> AMinerResearches { get; set; }
        public DbSet<MarauderResearch> MarauderResearches { get; set; }
        public DbSet<ViralResearch> ViralResearches { get; set; }
        public DbSet<CollectiveResearch> CollectiveResearches { get; set; }
        public DbSet<GuardianResearch> GuardianResearches { get; set; }
        public DbSet<ClusterResearch> ClusterResearches { get; set; }
        public DbSet<Ships> Ships { get; set; }
        public DbSet<Exploration> Explorations { get; set; }
        public DbSet<ImportantEvents> ImportantEvents { get; set; }
        public DbSet<BattleLogs> Battlelogs { get; set; }
        public DbSet<Federations> Federations { get; set; }
        public DbSet<FederationMessages> FederationMessages { get; set; }
        public DbSet<MarketPosts> MarketPosts { get; set; }
        public DbSet<FederationApplication> FederationApplications { get; set; }
        public DbSet<FederationWar> FederationWars { get; set; }
        public DbSet<NPCs> NPCs { get; set; }
        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //configure one-to-one relationship for Missions
            builder.Entity<ApplicationUser>()
                .HasOne(u => u.Missions)
                .WithOne(m => m.ApplicationUser)
                .HasForeignKey<Missions>(m => m.ApplicationUserId)
                .IsRequired();
            // Configure one-to-one relationship for UserProjects
            builder.Entity<ApplicationUser>()
                .HasOne(u => u.UserProjects)
                .WithOne(up => up.ApplicationUser)
                .HasForeignKey<UserProjects>(up => up.ApplicationUserId)
                .IsRequired();
            // Configure one-to-one relationship for Commodities
            builder.Entity<ApplicationUser>()
                .HasOne(u => u.Commodities)
                .WithOne(c => c.ApplicationUser)
                .HasForeignKey<Commodities>(c => c.ApplicationUserId)
                .IsRequired();
            // Configure one-to-many relationship for Fleet
            builder.Entity<Fleet>()
                .HasOne(f => f.ApplicationUser)
                .WithMany(u => u.Fleets)
                .HasForeignKey(f => f.ApplicationUserId)
                .IsRequired();
            // Configure one-to-many relationship for Planets
            builder.Entity<Planets>()
                .HasOne(f => f.ApplicationUser)
                .WithMany(u => u.Planets)
                .HasForeignKey(f => f.ApplicationUserId)
                .IsRequired();
            // Configure one-to-one relationship for Turns
            builder.Entity<Turns>()
                .HasOne(t => t.ApplicationUser)
                .WithOne(u => u.Turns)
                .HasForeignKey<Turns>(t => t.ApplicationUserId)
                .IsRequired();
            // Configure one-to-many relationship for Artifacts
            builder.Entity<Artifacts>()
                .HasOne(a => a.ApplicationUser)
                .WithMany(u => u.Artifacts)
                .HasForeignKey(a => a.ApplicationUserId)
                .IsRequired();
            // Configure one-to-one relationship for Infrastructer
            builder.Entity<Infrastructer>()
                .HasOne(i => i.ApplicationUser)
                .WithOne(u => u.Infrastructer)
                .HasForeignKey<Infrastructer>(i => i.ApplicationUserId)
                .IsRequired();
            // Configure one-to-one relationship for EClassResearch
            builder.Entity<EClassResearch>()
                .HasOne(e => e.ApplicationUser)
                .WithOne(u => u.EClassResearch)
                .HasForeignKey<EClassResearch>(e => e.ApplicationUserId)
                .IsRequired();
            // Configure one-to-one relationship for CyrilClassResearch
            builder.Entity<CyrilClassResearch>()
                .HasOne(c => c.ApplicationUser)
                .WithOne(u => u.CyrilClassResearch)
                .HasForeignKey<CyrilClassResearch>(c => c.ApplicationUserId)
                .IsRequired();
            // Configure one-to-one relationship for StrafezResearch
            builder.Entity<StrafezResearch>()
                .HasOne(s => s.ApplicationUser)
                .WithOne(u => u.StrafezResearch)
                .HasForeignKey<StrafezResearch>(s => s.ApplicationUserId)
                .IsRequired();
            // Configure one-to-one relationship for FClassResearch
            builder.Entity<FClassResearch>()
                .HasOne(f => f.ApplicationUser)
                .WithOne(u => u.FClassResearch)
                .HasForeignKey<FClassResearch>(f => f.ApplicationUserId)
                .IsRequired();
            // Configure one-to-one relationship for ProjectsResearch
            builder.Entity<ProjectsResearch>()
                .HasOne(p => p.ApplicationUser)
                .WithOne(u => u.ProjectsResearch)
                .HasForeignKey<ProjectsResearch>(p => p.ApplicationUserId)
                .IsRequired();
            // Configure one-to-one relationship for ViralSpecificResearch
            builder.Entity<ViralSpecificResearch>()
                .HasOne(v => v.ApplicationUser)
                .WithOne(u => u.ViralSpecificResearch)
                .HasForeignKey<ViralSpecificResearch>(v => v.ApplicationUserId)
                .IsRequired();
            // Configure one-to-one relationship for CollectiveSpecificResearch
            builder.Entity<CollectiveSpecificResearch>()
                .HasOne(c => c.ApplicationUser)
                .WithOne(u => u.CollectiveSpecificResearch)
                .HasForeignKey<CollectiveSpecificResearch>(c => c.ApplicationUserId)
                .IsRequired();
            // Configure one-to-one relationship for TerranResearch
            builder.Entity<TerranResearch>()
                .HasOne(t => t.ApplicationUser)
                .WithOne(u => u.TerranResearch)
                .HasForeignKey<TerranResearch>(t => t.ApplicationUserId)
                .IsRequired();
            // Configure one-to-one relationship for AMinerResearch
            builder.Entity<AMinerResearch>()
                .HasOne(a => a.ApplicationUser)
                .WithOne(u => u.AMinerResearch)
                .HasForeignKey<AMinerResearch>(a => a.ApplicationUserId)
                .IsRequired();
            // Configure one-to-one relationship for MarauderResearch
            builder.Entity<MarauderResearch>()
                .HasOne(m => m.ApplicationUser)
                .WithOne(u => u.MarauderResearch)
                .HasForeignKey<MarauderResearch>(m => m.ApplicationUserId)
                .IsRequired();
            // Configure one-to-one relationship for ViralResearch
            builder.Entity<ViralResearch>()
                .HasOne(v => v.ApplicationUser)
                .WithOne(u => u.ViralResearch)
                .HasForeignKey<ViralResearch>(v => v.ApplicationUserId)
                .IsRequired();
            // Configure one-to-one relationship for CollectiveResearch
            builder.Entity<CollectiveResearch>()
                .HasOne(c => c.ApplicationUser)
                .WithOne(u => u.CollectiveResearch)
                .HasForeignKey<CollectiveResearch>(c => c.ApplicationUserId)
                .IsRequired();
            // Configure one-to-one relationship for GuardianResearch
            builder.Entity<GuardianResearch>()
                .HasOne(g => g.ApplicationUser)
                .WithOne(u => u.GuardianResearch)
                .HasForeignKey<GuardianResearch>(g => g.ApplicationUserId)
                .IsRequired();
            // Configure one-to-one relationship for ClusterResearch
            builder.Entity<ClusterResearch>()
                .HasOne(c => c.ApplicationUser)
                .WithOne(u => u.ClusterResearch)
                .HasForeignKey<ClusterResearch>(c => c.ApplicationUserId)
                .IsRequired();
            // Configure one-to-one relationship for Exploration
            builder.Entity<Exploration>()
                .HasOne(e => e.ApplicationUser)
                .WithOne(u => u.Exploration)
                .HasForeignKey<Exploration>(e => e.ApplicationUserId)
                .IsRequired();
            // Configure one-to-many relationship for ImportantEvents
            builder.Entity<ImportantEvents>()
                .HasOne(ie => ie.ApplicationUser)
                .WithMany(u => u.ImportantEvents)
                .HasForeignKey(ie => ie.ApplicationUserId)
                .IsRequired();
            // Configure one-to-many relationship for BattleLogs
            builder.Entity<BattleLogs>()
                .HasOne(bl => bl.ApplicationUser)
                .WithMany(u => u.Battlelogs)
                .HasForeignKey(bl => bl.ApplicationUserId)
                .IsRequired();
            // Federation Leader (One-to-Many)
            builder.Entity<Federations>()
                .HasOne(f => f.FederationLeader)
                .WithMany()
                .HasForeignKey(f => f.FederationLeaderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Federation Members (One-to-Many)
            builder.Entity<Federations>()
                .HasMany(f => f.FederationMembers)
                .WithOne(u => u.Federation)
                .HasForeignKey(u => u.FederationId)
                .OnDelete(DeleteBehavior.SetNull);

            // Federation Wars (Self-Referencing Many-to-Many)
            builder.Entity<Federations>()
                .HasMany(f => f.FederationWars)
                .WithMany();

            builder.Entity<FederationWar>()
                .HasOne(fw => fw.AttackerFederation)
                .WithMany()
                .HasForeignKey(fw => fw.AttackerFederationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<FederationWar>()
                .HasOne(fw => fw.DefenderFederation)
                .WithMany()
                .HasForeignKey(fw => fw.DefenderFederationId)
                .OnDelete(DeleteBehavior.Restrict);

            // Federation Discussion (One-to-Many)
            builder.Entity<Federations>()
                .HasMany(f => f.FederationDiscussion)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            // FederationApplication: One-to-One (User) and One-to-Many (Federation)
            builder.Entity<ApplicationUser>()
                .HasOne(u => u.FederationApplication)
                .WithOne(a => a.ApplicationUser)
                .HasForeignKey<FederationApplication>(a => a.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Federations>()
                .HasMany(f => f.FederationApplicants)
                .WithOne(a => a.Federation)
                .HasForeignKey(a => a.FederationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Federation Activity (One-to-Many)
            builder.Entity<Federations>()
                .HasMany(f => f.FederationActivity)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            // FederationMessages Sender (Many-to-One)
            builder.Entity<FederationMessages>()
                .HasOne(m => m.Federation)
                .WithMany(f => f.FederationDiscussion)
                .HasForeignKey(m => m.FederationId)
                .OnDelete(DeleteBehavior.Cascade);
            // ApplicationUser Federation relationship
            builder.Entity<ApplicationUser>()
                .HasOne(u => u.Federation)
                .WithMany(f => f.FederationMembers)
                .HasForeignKey(u => u.FederationId)
                .OnDelete(DeleteBehavior.SetNull);
            // MarketPosts relationship
            builder.Entity<MarketPosts>()
                .HasOne(mp => mp.ApplicationUser)
                .WithMany(u => u.MarketPosts)
                .HasForeignKey(mp => mp.ApplicationUserId);
            // configure one-to-many relationship for private messages
            builder.Entity<ApplicationUser>()
                .HasMany(u => u.PrivateMessagesSent)
                .WithOne(pm => pm.Sender)
                .HasForeignKey(pm => pm.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PrivateMessage>()
                .HasOne(pm => pm.Sender)
                .WithMany()
                .HasForeignKey(pm => pm.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PrivateMessage>()
                .HasOne(pm => pm.Receiver)
                .WithMany()
                .HasForeignKey(pm => pm.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);
            // configure one-to-many relationship for counterattacks
            builder.Entity<ApplicationUser>()
                .HasMany(u => u.CounterAttacks)
                .WithOne(ca => ca.ApplicationUser)
                .HasForeignKey(ca => ca.ApplicationUserId)
                .IsRequired();
            // configure one-to-one relationship for ViralReversedShips
            builder.Entity<ViralReversedShips>()
                .HasOne(v => v.ApplicationUser)
                .WithOne(u => u.ViralReversedShips)
                .HasForeignKey<ViralReversedShips>(v => v.ApplicationUserId)
                .IsRequired();
            //// Seed NPCs

            //var npcNames = new[]
            //{
            //"Aerwyn", "Beldorn", "Corlith", "Dranor", "Eldras", "Fenwyn", "Galmir", "Helnar", "Ithril", "Jarlorn",
            //"Kelthas", "Lormir", "Mornas", "Noryn", "Orvyr", "Perion", "Quelthil", "Ryndras", "Sorlorn", "Torwyn",
            //"Ulnar", "Valmir", "Wynras", "Xanthil", "Yorion", "Zeldras", "Aerthas", "Belwyn", "Corwyn", "Drasor",
            //"Eldwyn", "Fenmir", "Galnar", "Helwyn", "Ithras", "Jarmir", "Kelwyn", "Lornas", "Mornor", "Noryra",
            //"Ormir", "Pernar", "Quelmir", "Rynwyn", "Sornar", "Torlith", "Ulwyn", "Valnar", "Wynmir", "Xanwyn",
            //"Yorthas", "Zelwyn", "Aermir", "Beldras", "Corwyn", "Dranor", "Eldras", "Fenwyn", "Galmir", "Helnar",
            //"Ithril", "Jarlorn", "Kelthas", "Lormir", "Mornas", "Noryn", "Orvyr", "Perion", "Quelthil", "Ryndras",
            //"Sorlorn", "Torwyn", "Ulnar", "Valmir", "Wynras", "Xanthil", "Yorion", "Zeldras", "Aerthas", "Belwyn",
            //"Corwyn", "Drasor", "Eldwyn", "Fenmir", "Galnar", "Helwyn", "Ithras", "Jarmir", "Kelwyn", "Lornas",
            //"Mornor", "Noryra", "Ormir", "Pernar", "Quelmir", "Rynwyn", "Sornar", "Torlith"
            //};

            //var npcs = new List<NPCs>();
            //var random = new Random(42); // Use a fixed seed for deterministic results

            //for (int i = 0; i < npcNames.Length; i++)
            //{
            //    npcs.Add(new NPCs
            //    {
            //        Id = i + 1,
            //        UserName = npcNames[i],
            //        PowerRating = random.Next(15000, 2000000),
            //        PlayingSince = DateTime.UtcNow,
            //        EmpireAge = 0,
            //        BattlesWon = 0,
            //        BattlesLost = 0,
            //        ColoniesWon = 0,
            //        ColoniesLost = 0,
            //        ColoniesExplored = 0,
            //        PlanetsPlundered = 0,
            //        Faction = (Faction)random.Next(Enum.GetValues(typeof(Faction)).Length),
            //        TotalColonies = 0,
            //        TotalPlanets = 0,
            //        DamageProtection = DateTime.UtcNow,
            //        LastAction = DateTime.UtcNow,
            //        ArtifactShield = 0.0m,
            //        IsNPC = true
            //    });
            //}

            //builder.Entity<NPCs>().HasData(npcs);

        }
    }
}