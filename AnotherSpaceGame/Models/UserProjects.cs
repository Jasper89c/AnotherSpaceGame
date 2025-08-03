namespace AnotherSpaceGame.Models
{
    public class UserProjects
    {
        public int Id { get; set; }
        public bool CapsuleLab { get; set; }
        public int CapsuleLabTurnsRequired { get; set; }
        public int CapsuleLabCreditsRequired { get; set; }
        public DateTime CapsuleLabUnlockTimer { get; set; }

        public bool KalZulLoktar { get; set; }
        public int KalZulLoktarTurnsRequired { get; set; }
        public int KalZulLoktarCreditsRequired { get; set; }
        public DateTime KalZulLoktarUnlockTimer { get; set; }

        public bool KalZulHektar { get; set; }
        public int KalZulHektarTurnsRequired { get; set; }
        public int KalZulHektarCreditsRequired { get; set; }
        public DateTime KalZulHektarUnlockTimer { get; set; }

        public bool Itech { get; set; }
        public int ItechTurnsRequired { get; set; }
        public int ItechCreditsRequired { get; set; }
        public DateTime ItechUnlockTimer { get; set; }

        public bool UnreverseEngineering { get; set; }
        public int UnreverseEngineeringTurnsRequired { get; set; }
        public int UnreverseEngineeringCreditsRequired { get; set; }
        public DateTime UnreverseEngineeringUnlockTimer { get; set; }
        public bool AdvancedExploration { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public UserProjects()
        {
            CapsuleLabTurnsRequired = 450;
            CapsuleLabCreditsRequired = 250000000;
            CapsuleLabUnlockTimer = DateTime.MinValue;
            KalZulLoktarTurnsRequired = 200;
            KalZulLoktarCreditsRequired = 400000000;
            KalZulLoktarUnlockTimer = DateTime.MinValue;
            KalZulHektarTurnsRequired = 400;
            KalZulHektarCreditsRequired = 800000000;
            KalZulHektarUnlockTimer = DateTime.MinValue;
            ItechTurnsRequired = 150;
            ItechCreditsRequired = 250000000;
            ItechUnlockTimer = DateTime.MinValue;
            UnreverseEngineeringTurnsRequired = 100;
            UnreverseEngineeringCreditsRequired = 100000000;
            UnreverseEngineeringUnlockTimer = DateTime.MinValue;
        }
    }
}
