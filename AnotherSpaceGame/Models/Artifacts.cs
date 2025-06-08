using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnotherSpaceGame.Models
{
    public class Artifacts
    {
        [Key]
        public int Id { get; set; }

        // Foreign key to ApplicationUser
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int ArtifactId { get; set; }
        public ArtifactName ArtifactName { get; set; }
        public ArtifactType ArtifactType { get; set; }
        public int Total { get; set; }
        public int MaxTotal { get; set; }

        public Artifacts() { }
        public Artifacts(int artifactId, int totalToAdd, string applicationUserId)
        {
            ApplicationUserId = applicationUserId;
            Total = totalToAdd + Total;
            switch (artifactId)
            {
                case 1:
                    ArtifactId = 1;
                    ArtifactName = ArtifactName.AquaOrb;
                    ArtifactType = ArtifactType.Common;
                    MaxTotal = 255;
                    break;
                case 2:
                    ArtifactId = 2;
                    ArtifactName = ArtifactName.BlackOrb;
                    ArtifactType = ArtifactType.Common;
                    MaxTotal = 255;
                    break;
                case 3:
                    ArtifactId = 3;
                    ArtifactName = ArtifactName.BlueOrb;
                    ArtifactType = ArtifactType.Common;
                    MaxTotal = 255;
                    break;
                case 4:
                    ArtifactId = 4;
                    ArtifactName = ArtifactName.BrownOrb;
                    ArtifactType = ArtifactType.Common;
                    MaxTotal = 255;
                    break;
                case 5:
                    ArtifactId = 5;
                    ArtifactName = ArtifactName.EnergyPod;
                    ArtifactType = ArtifactType.Common;
                    MaxTotal = 255;
                    break;
                case 6:
                    ArtifactId = 6;
                    ArtifactName = ArtifactName.GoldenOrb;
                    ArtifactType = ArtifactType.Common;
                    MaxTotal = 255;
                    break;
                case 7:
                    ArtifactId = 7;
                    ArtifactName = ArtifactName.GrayOrb;
                    ArtifactType = ArtifactType.Common;
                    MaxTotal = 255;
                    break;
                case 8:
                    ArtifactId = 8;
                    ArtifactName = ArtifactName.GreenOrb;
                    ArtifactType = ArtifactType.Common;
                    MaxTotal = 255;
                    break;
                case 9:
                    ArtifactId = 9;
                    ArtifactName = ArtifactName.MoccasinOrb;
                    ArtifactType = ArtifactType.Common;
                    MaxTotal = 255;
                    break;
                case 10:
                    ArtifactId = 10;
                    ArtifactName = ArtifactName.OrangeOrb;
                    ArtifactType = ArtifactType.Common;
                    MaxTotal = 255;
                    break;
                case 11:
                    ArtifactId = 11;
                    ArtifactName = ArtifactName.PinkOrb;
                    ArtifactType = ArtifactType.Common;
                    MaxTotal = 255;
                    break;
                case 12:
                    ArtifactId = 12;
                    ArtifactName = ArtifactName.PlumOrb;
                    ArtifactType = ArtifactType.Common;
                    MaxTotal = 255;
                    break;
                case 13:
                    ArtifactId = 13;
                    ArtifactName = ArtifactName.PurpleOrb;
                    ArtifactType = ArtifactType.Common;
                    MaxTotal = 255;
                    break;
                case 14:
                    ArtifactId = 14;
                    ArtifactName = ArtifactName.TurquoiseOrb;
                    ArtifactType = ArtifactType.Common;
                    MaxTotal = 255;
                    break;
                case 15:
                    ArtifactId = 15;
                    ArtifactName = ArtifactName.WhiteOrb;
                    ArtifactType = ArtifactType.Common;
                    MaxTotal = 255;
                    break;
                case 16:
                    ArtifactId = 16;
                    ArtifactName = ArtifactName.YellowOrb;
                    ArtifactType = ArtifactType.Common;
                    MaxTotal = 255;
                    break;
                case 17:
                    ArtifactId = 17;
                    ArtifactName = ArtifactName.AmberDinero;
                    ArtifactType = ArtifactType.Uncommon;
                    MaxTotal = 100;
                    break;
                case 18:
                    ArtifactId = 18;
                    ArtifactName = ArtifactName.AmethystDinero;
                    ArtifactType = ArtifactType.Uncommon;
                    MaxTotal = 100;
                    break;
                case 19:
                    ArtifactId = 19;
                    ArtifactName = ArtifactName.AssimilatedBase;
                    ArtifactType = ArtifactType.Uncommon;
                    MaxTotal = 100;
                    break;
                case 20:
                    ArtifactId = 20;
                    ArtifactName = ArtifactName.BronzeDinero;
                    ArtifactType = ArtifactType.Uncommon;
                    MaxTotal = 100;
                    break;
                case 21:
                    ArtifactId = 21;
                    ArtifactName = ArtifactName.CuartoMapa;
                    ArtifactType = ArtifactType.Uncommon;
                    MaxTotal = 100;
                    break;
                case 22:
                    ArtifactId = 22;
                    ArtifactName = ArtifactName.GarnetDinero;
                    ArtifactType = ArtifactType.Uncommon;
                    MaxTotal = 100;
                    break;
                case 23:
                    ArtifactId = 23;
                    ArtifactName = ArtifactName.GoldDinero;
                    ArtifactType = ArtifactType.Uncommon;
                    MaxTotal = 100;
                    break;
                case 24:
                    ArtifactId = 24;
                    ArtifactName = ArtifactName.OpalDinero;
                    ArtifactType = ArtifactType.Uncommon;
                    MaxTotal = 100;
                    break;
                case 25:
                    ArtifactId = 25;
                    ArtifactName = ArtifactName.OrganicBase;
                    ArtifactType = ArtifactType.Uncommon;
                    MaxTotal = 100;
                    break;
                case 26:
                    ArtifactId = 26;
                    ArtifactName = ArtifactName.PlatinumDinero;
                    ArtifactType = ArtifactType.Uncommon;
                    MaxTotal = 100;
                    break;
                case 27:
                    ArtifactId = 27;
                    ArtifactName = ArtifactName.SilverDinero;
                    ArtifactType = ArtifactType.Uncommon;
                    MaxTotal = 100;
                    break;
                case 28:
                    ArtifactId = 28;
                    ArtifactName = ArtifactName.TopazDinero;
                    ArtifactType = ArtifactType.Uncommon;
                    MaxTotal = 100;
                    break;
                case 29:
                    ArtifactId = 29;
                    ArtifactName = ArtifactName.MajorSuerte;
                    ArtifactType = ArtifactType.Rare;
                    MaxTotal = 50;
                    break;
                case 30:
                    ArtifactId = 30;
                    ArtifactName = ArtifactName.MinorAlimento;
                    ArtifactType = ArtifactType.Rare;
                    MaxTotal = 50;
                    break;
                case 31:
                    ArtifactId = 31;
                    ArtifactName = ArtifactName.MinorBarrera;
                    ArtifactType = ArtifactType.Rare;
                    MaxTotal = 50;
                    break;
                case 32:
                    ArtifactId = 32;
                    ArtifactName = ArtifactName.MinorCosecha;
                    ArtifactType = ArtifactType.Rare;
                    MaxTotal = 50;
                    break;
                case 33:
                    ArtifactId = 33;
                    ArtifactName = ArtifactName.MinorGente;
                    ArtifactType = ArtifactType.Rare;
                    MaxTotal = 50;
                    break;
                case 34:
                    ArtifactId = 34;
                    ArtifactName = ArtifactName.MinorRequerido;
                    ArtifactType = ArtifactType.Rare;
                    MaxTotal = 50;
                    break;
                case 35:
                    ArtifactId = 35;
                    ArtifactName = ArtifactName.MinorSuerte;
                    ArtifactType = ArtifactType.Rare;
                    MaxTotal = 50;
                    break;
                case 36:
                    ArtifactId = 36;
                    ArtifactName = ArtifactName.MinorTierra;
                    ArtifactType = ArtifactType.Rare;
                    MaxTotal = 50;
                    break;
                case 37:
                    ArtifactId = 37;
                    ArtifactName = ArtifactName.SmallTimeCapsule;
                    ArtifactType = ArtifactType.Rare;
                    MaxTotal = 50;
                    break;
                case 38:
                    ArtifactId = 38;
                    ArtifactName = ArtifactName.Traicione;
                    ArtifactType = ArtifactType.Rare;
                    MaxTotal = 50;
                    break;
                case 39:
                    ArtifactId = 39;
                    ArtifactName = ArtifactName.BigTimeCapsule;
                    ArtifactType = ArtifactType.Unique;
                    MaxTotal = 25;
                    break;
                case 40:
                    ArtifactId = 40;
                    ArtifactName = ArtifactName.Historia;
                    ArtifactType = ArtifactType.Unique;
                    MaxTotal = 25;
                    break;
                case 41:
                    ArtifactId = 41;
                    ArtifactName = ArtifactName.MajorAfortunado;
                    ArtifactType = ArtifactType.Unique;
                    MaxTotal = 25;
                    break;
                case 42:
                    ArtifactId = 42;
                    ArtifactName = ArtifactName.MajorAlimento;
                    ArtifactType = ArtifactType.Unique;
                    MaxTotal = 25;
                    break;
                case 43:
                    ArtifactId = 43;
                    ArtifactName = ArtifactName.MajorBarrera;
                    ArtifactType = ArtifactType.Unique;
                    MaxTotal = 25;
                    break;
                case 44:
                    ArtifactId = 44;
                    ArtifactName = ArtifactName.MajorCosecha;
                    ArtifactType = ArtifactType.Unique;
                    MaxTotal = 25;
                    break;
                case 45:
                    ArtifactId = 45;
                    ArtifactName = ArtifactName.MajorDinero;
                    ArtifactType = ArtifactType.Unique;
                    MaxTotal = 25;
                    break;
                case 46:
                    ArtifactId = 46;
                    ArtifactName = ArtifactName.MajorProducto;
                    ArtifactType = ArtifactType.Unique;
                    MaxTotal = 25;
                    break;
                case 47:
                    ArtifactId = 47;
                    ArtifactName = ArtifactName.MajorTierra;
                    ArtifactType = ArtifactType.Unique;
                    MaxTotal = 25;
                    break;
                case 48:
                    ArtifactId = 48;
                    ArtifactName = ArtifactName.MinorAfortunado;
                    ArtifactType = ArtifactType.Unique;
                    MaxTotal = 25;
                    break;
                case 49:
                    ArtifactId = 49;
                    ArtifactName = ArtifactName.MinorEstructura;
                    ArtifactType = ArtifactType.Unique;
                    MaxTotal = 25;
                    break;
                case 50:
                    ArtifactId = 50;
                    ArtifactName = ArtifactName.MinorGordo;
                    ArtifactType = ArtifactType.Unique;
                    MaxTotal = 25;
                    break;
                case 51:
                    ArtifactId = 51;
                    ArtifactName = ArtifactName.Persiana;
                    ArtifactType = ArtifactType.Unique;
                    MaxTotal = 25;
                    break;
                case 52:
                    ArtifactId = 52;
                    ArtifactName = ArtifactName.Regalo;
                    ArtifactType = ArtifactType.Unique;
                    MaxTotal = 25;
                    break;
                case 53:
                    ArtifactId = 53;
                    ArtifactName = ArtifactName.GrandAlimenter;
                    ArtifactType = ArtifactType.Special;
                    MaxTotal = 10;
                    break;
                case 54:
                    ArtifactId = 54;
                    ArtifactName = ArtifactName.GrandAlimento;
                    ArtifactType = ArtifactType.Special;
                    MaxTotal = 10;
                    break;
                case 55:
                    ArtifactId = 55;
                    ArtifactName = ArtifactName.GrandBarrera;
                    ArtifactType = ArtifactType.Special;
                    MaxTotal = 10;
                    break;
                case 56:
                    ArtifactId = 56;
                    ArtifactName = ArtifactName.GrandCosecha;
                    ArtifactType = ArtifactType.Special;
                    MaxTotal = 10;
                    break;
                case 57:
                    ArtifactId = 57;
                    ArtifactName = ArtifactName.GrandDinero;
                    ArtifactType = ArtifactType.Special;
                    MaxTotal = 10;
                    break;
                case 58:
                    ArtifactId = 58;
                    ArtifactName = ArtifactName.GrandEstructura;
                    ArtifactType = ArtifactType.Special;
                    MaxTotal = 10;
                    break;
                case 59:
                    ArtifactId = 59;
                    ArtifactName = ArtifactName.GrandGente;
                    ArtifactType = ArtifactType.Special;
                    MaxTotal = 10;
                    break;
                case 60:
                    ArtifactId = 60;
                    ArtifactName = ArtifactName.GrandProducto;
                    ArtifactType = ArtifactType.Special;
                    MaxTotal = 10;
                    break;
                case 61:
                    ArtifactId = 61;
                    ArtifactName = ArtifactName.GrandRequerido;
                    ArtifactType = ArtifactType.Special;
                    MaxTotal = 10;
                    break;
                case 62:
                    ArtifactId = 62;
                    ArtifactName = ArtifactName.GrandTierra;
                    ArtifactType = ArtifactType.Special;
                    MaxTotal = 10;
                    break;
                case 63:
                    ArtifactId = 63;
                    ArtifactName = ArtifactName.MajorGordo;
                    ArtifactType = ArtifactType.Special;
                    MaxTotal = 10;
                    break;
                case 64:
                    ArtifactId = 64;
                    ArtifactName = ArtifactName.PlanetaryCore;
                    ArtifactType = ArtifactType.Special;
                    MaxTotal = 10;
                    break;
                default:
                    break;
            }
        }
    }
}