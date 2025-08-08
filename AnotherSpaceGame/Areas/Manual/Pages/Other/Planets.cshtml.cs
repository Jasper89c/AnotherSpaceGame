using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AnotherSpaceGame.Areas.Manual.Pages.Other
{
    public class PlanetsModel : PageModel
    {
        public List<DisplayPlanet> Planets { get; set; } = new List<DisplayPlanet>();

        public PlanetsModel()
        {
        }

        public void OnGet()
        {
            foreach (PlanetType item in Enum.GetValues(typeof(PlanetType)))
            {
                Planets.Add(DisplayPlanet.FromPlanetType(item)); // Fixed: Correctly call the static method FromPlanetType
            }
        }
    }

    public class DisplayPlanet
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PopulationGrowth { get; set; }
        public string AgricultureModifier { get; set; }
        public string ArtifactModifier { get; set; }
        public string LandSize { get; set; }
        public string OreSize { get; set; }

        public static DisplayPlanet FromPlanetType(PlanetType type)
        {
            return type switch
            {
                PlanetType.Barren => new DisplayPlanet
                {
                    Name = "Barren",
                    Description = "This planet generally has thin atmosphere and relatively flat. This would make it a perfect place for construction.",
                    PopulationGrowth = "-50%",
                    AgricultureModifier = "-100%",
                    ArtifactModifier = "-99%",
                    LandSize = "50-1,100",
                    OreSize = "150-500"
                },
                PlanetType.Icy => new DisplayPlanet
                {
                    Name = "Icy",
                    Description = "The entire planet is covered with thick layer of ice with a outside temprature of -500 to -200 celsious. Ore is found in abundance but mining can prove challenging due to the extreme weather, blizzards and occasional hail storms.",
                    PopulationGrowth = "-25%",
                    AgricultureModifier = "0%",
                    ArtifactModifier = "-99%",
                    LandSize = "24-83",
                    OreSize = "1,500-4,500"
                },
                PlanetType.Marshy => new DisplayPlanet
                {
                    Name = "Marshy",
                    Description = "Marshy land does not provide a suitable building foundation for construction building. Although it is possible to somewhat farm here due to the existance of water, the outcome of crops is generally low quality as there is a considerable lack of arable land.",
                    PopulationGrowth = "-20%",
                    AgricultureModifier = "-50%",
                    ArtifactModifier = "-99%",
                    LandSize = "50-150",
                    OreSize = "0-150"
                },
                PlanetType.Forest => new DisplayPlanet
                {
                    Name = "Forest",
                    Description = "Correct amounts of rain water and proper soil makes this planet an ideal place for agriculture. Traces of ore can be found but would first require clearing the existing vegitation for mining. Due to the large amount of forests, it is hard to set up housing without clearing some parts via slash-and-burn.",
                    PopulationGrowth = "-20%",
                    AgricultureModifier = "-50%",
                    ArtifactModifier = "-99%",
                    LandSize = "50-150",
                    OreSize = "0-150"
                },
                PlanetType.Oceanic => new DisplayPlanet
                {
                    Name = "Oceanic",
                    Description = "This entire planet is covered with water, and hence construction is only possible by building underwater. Agriculture cannott be done here because there is no suitable ground for it. Might prove useless, but what secrets does it hide…?",
                    PopulationGrowth = "-10%",
                    AgricultureModifier = "-100%",
                    ArtifactModifier = "-90%",
                    LandSize = "10-50",
                    OreSize = "0"
                },
                PlanetType.Rocky => new DisplayPlanet
                {
                    Name = "Rocky",
                    Description = "This is one of the richest planet type due to the vast amounts of ore can be found on the surface.",
                    PopulationGrowth = "-25%",
                    AgricultureModifier = "0%",
                    ArtifactModifier = "-99.99%",
                    LandSize = "34-121",
                    OreSize = "500-3,300"
                },
                PlanetType.Desert => new DisplayPlanet
                {
                    Name = "Desert",
                    Description = "This planet is covered by Blood Red Sands for as far as the eyes can see. Mining is simple as ores can be found under the sand. This makes this type of planet the easiest to mine, however anything on sand might get eroded over time.",
                    PopulationGrowth = "-25%",
                    AgricultureModifier = "-25%",
                    ArtifactModifier = "-99%",
                    LandSize = "100-250",
                    OreSize = "100-350"
                },
                PlanetType.Balanced => new DisplayPlanet
                {
                    Name = "Balanced",
                    Description = "This is an M class planet which is an Earth type planet. It has a balanced water, land mass and mineral reserve, making it ideal as a hospitable homeworld to most empires.",
                    PopulationGrowth = "+20%",
                    AgricultureModifier = "0%",
                    ArtifactModifier = "-95%",
                    LandSize = "185-1,050",
                    OreSize = "750-1,100"
                },
                PlanetType.Gas => new DisplayPlanet
                {
                    Name = "Gas",
                    Description = "Since the entire planet is a big ball of gas there is no possible way for constructiong, mining or agriculture. Small asteroids and moons provide very limited landmass for construction.",
                    PopulationGrowth = "0%",
                    AgricultureModifier = "-100%",
                    ArtifactModifier = "-95%",
                    LandSize = "2-6",
                    OreSize = "0"
                },
                PlanetType.URich => new DisplayPlanet
                {
                    Name = "U.Rich",
                    Description = "Rich class planets is planets with out an atmosphere. This would give it a harsh lifeless environment, unable to support much life let alone agricultural. Although mining may prove to be tricky, this planet are known to be the richest planet in the known galaxy. Ore is abundance every where on the under the surface of the planet. This planet can not be clustered, infected or assimilated.",
                    PopulationGrowth = "-90%",
                    AgricultureModifier = "-100%",
                    ArtifactModifier = "-99%",
                    LandSize = "11-28",
                    OreSize = "50,000-350,000"
                },
                PlanetType.UEden => new DisplayPlanet
                {
                    Name = "U.Eden",
                    Description = "Eden class planets is planets with a thick atmosphere along with rich oxygen levels suitable to support life. Population thrives very well on this planet with astronomical growth rates. However due to the high level of oxygen, all ore on the planet has been oxidized rendering it useless for use. Agriculture also suffers greatly due to the high level of oxygen and low level of carbon dioxide. Ore is abundance every where on the under the surface of the planet.",
                    PopulationGrowth = "1000%",
                    AgricultureModifier = "-98%",
                    ArtifactModifier = "-99%",
                    LandSize = "500-2,500",
                    OreSize = "0"
                },
                PlanetType.USpazial => new DisplayPlanet
                {
                    Name = "U.Spazial",
                    Description = "This Spazial class planets still puzzles is scientists of its actual usage. This planet has extremely dense nitrogen atmosphere which makes it lifeless. Asteroids are generally found arond this planet forming a eclipsed shaped ring which encircles the planet. Outpost can still be build on this small asteroids however the build up area is limited. Ore is abundance every where on the under the surface of the planet. This planet can not be clustered, infected or assimilated.",
                    PopulationGrowth = "-90%",
                    AgricultureModifier = "-100%",
                    ArtifactModifier = "-85%",
                    LandSize = "2-4",
                    OreSize = "0"
                },
                PlanetType.ULarge => new DisplayPlanet
                {
                    Name = "U.Large",
                    Description = "Large class planets is basically an extremely huge and large barren planet with the largest mass known in the galaxy. The massive mass provides a very large area for construction of buildings making this the best planet to construct infrastructure. Imports of food may be required for this planet mainly due to the low atmospheric conditions which makes it unable to support agriculture. This planet can not be clustered, infected or assimilated.",
                    PopulationGrowth = "-80%",
                    AgricultureModifier = "-100%",
                    ArtifactModifier = "-99%",
                    LandSize = "7,000-16,000",
                    OreSize = "0"
                },
                PlanetType.UFertile => new DisplayPlanet
                {
                    Name = "U.Fertile",
                    Description = "Fertile class planets consist of a moist atmosphere rich in carbon dioxide making is ideal for agriculture. The high amounts of carbon dioxide is poisonous to other life forms therefore restrict growth of occupants. This planet can not be clustered, infected or assimilated.",
                    PopulationGrowth = "-50%",
                    AgricultureModifier = "+175%",
                    ArtifactModifier = "-99%",
                    LandSize = "950-1,250",
                    OreSize = "0"
                },
                PlanetType.Dead => new DisplayPlanet
                {
                    Name = "Dead",
                    Description = "This empty and lifeless planet has been drained of all mineral resources. There is not much use for this planet other than to be used to dump garbage or industrial toxic materials. This planet can not be clustered, infected or assimilated.",
                    PopulationGrowth = "-95%",
                    AgricultureModifier = "-100%",
                    ArtifactModifier = "-99%",
                    LandSize = "2-4",
                    OreSize = "0"
                },
                PlanetType.SimilareC1 => new DisplayPlanet
                {
                    Name = "Similare C.1",
                    Description = "Planet which has been assimilated by the Collective Race",
                    PopulationGrowth = "+20%",
                    AgricultureModifier = "-70%",
                    ArtifactModifier = "-99%",
                    LandSize = "Based on planets clustered",
                    OreSize = "Based on planets clustered"
                },
                PlanetType.SimilareC2 => new DisplayPlanet
                {
                    Name = "Similare C.2",
                    Description = "Planet which has been assimilated by the Collective Race. This is a planetary upgrade which consists of 4 Similare C.1",
                    PopulationGrowth = "+20%",
                    AgricultureModifier = "-65%",
                    ArtifactModifier = "-99%",
                    LandSize = "Based on planets clustered",
                    OreSize = "Based on planets clustered"
                },
                PlanetType.SimilareC3 => new DisplayPlanet
                {
                    Name = "Similare C.3",
                    Description = "Planet which has been assimilated by the Collective Race. This is a planetary upgrade which consists of 4 Similare C.2",
                    PopulationGrowth = "+20%",
                    AgricultureModifier = "-60%",
                    ArtifactModifier = "-99%",
                    LandSize = "Based on planets clustered",
                    OreSize = "Based on planets clustered"
                },
                PlanetType.SimilareC4 => new DisplayPlanet
                {
                    Name = "Similare C.4",
                    Description = "Planet which has been assimilated by the Collective Race. This is a planetary upgrade which consists of 4 Similare C.3",
                    PopulationGrowth = "+20%",
                    AgricultureModifier = "-55%",
                    ArtifactModifier = "-99%",
                    LandSize = "Based on planets clustered",
                    OreSize = "Based on planets clustered"
                },
                PlanetType.SimilareC5 => new DisplayPlanet
                {
                    Name = "Similare C.5",
                    Description = "Planet which has been assimilated by the Collective Race. This is a planetary upgrade which consists of 4 Similare C.4",
                    PopulationGrowth = "+20%",
                    AgricultureModifier = "-52%",
                    ArtifactModifier = "-99%",
                    LandSize = "Based on planets clustered",
                    OreSize = "Based on planets clustered"
                },
                PlanetType.AssimilatedC1 => new DisplayPlanet
                {
                    Name = "Assimilated C.1",
                    Description = "Cluster Lvl 1 which has been assimilated by the Collective Race",
                    PopulationGrowth = "0%",
                    AgricultureModifier = "-20%",
                    ArtifactModifier = "-99%",
                    LandSize = "Based on planets clustered",
                    OreSize = "Based on planets clustered"
                },
                PlanetType.AssimilatedC2 => new DisplayPlanet
                {
                    Name = "Assimilated C.2",
                    Description = "Cluster Lvl 2 which has been assimilated by the Collective Race",
                    PopulationGrowth = "0%",
                    AgricultureModifier = "-10%",
                    ArtifactModifier = "-99%",
                    LandSize = "Based on planets clustered",
                    OreSize = "Based on planets clustered"
                },
                PlanetType.AssimilatedC3 => new DisplayPlanet
                {
                    Name = "Assimilated C.3",
                    Description = "Cluster Lvl 3 which has been assimilated by the Collective Race",
                    PopulationGrowth = "0%",
                    AgricultureModifier = "0%",
                    ArtifactModifier = "-99%",
                    LandSize = "Based on planets clustered",
                    OreSize = "Based on planets clustered"
                },
                PlanetType.TaintedC1 => new DisplayPlanet
                {
                    Name = "Tainted C.1",
                    Description = "Planet which has been infected by the Viral Race",
                    PopulationGrowth = "-20%",
                    AgricultureModifier = "-20%",
                    ArtifactModifier = "-99%",
                    LandSize = "Based on planets clustered",
                    OreSize = "Based on planets clustered"
                },
                PlanetType.TaintedC2 => new DisplayPlanet
                {
                    Name = "Tainted C.2",
                    Description = "Planet which has been infected by the Viral Race. This is a planetary upgrade which consists of 4 Tainted C.1",
                    PopulationGrowth = "-30%",
                    AgricultureModifier = "-25%",
                    ArtifactModifier = "-99%",
                    LandSize = "Based on planets clustered",
                    OreSize = "Based on planets clustered"
                },
                PlanetType.TaintedC3 => new DisplayPlanet
                {
                    Name = "Tainted C.3",
                    Description = "Planet which has been infected by the Viral Race. This is a planetary upgrade which consists of 4 Tainted C.2",
                    PopulationGrowth = "-40%",
                    AgricultureModifier = "-30%",
                    ArtifactModifier = "-99%",
                    LandSize = "Based on planets clustered",
                    OreSize = "Based on planets clustered"
                },
                PlanetType.TaintedC4 => new DisplayPlanet
                {
                    Name = "Tainted C.4",
                    Description = "Planet which has been infected by the Viral Race. This is a planetary upgrade which consists of 4 Tainted C.3",
                    PopulationGrowth = "-50%",
                    AgricultureModifier = "-35%",
                    ArtifactModifier = "-99%",
                    LandSize = "Based on planets clustered",
                    OreSize = "Based on planets clustered"
                },
                PlanetType.InfectedC1 => new DisplayPlanet
                {
                    Name = "Infect C.1",
                    Description = "Cluster Lvl 1 which has been infected by the Viral Race",
                    PopulationGrowth = "-30%",
                    AgricultureModifier = "-10%",
                    ArtifactModifier = "-99%",
                    LandSize = "Based on planets clustered",
                    OreSize = "Based on planets clustered"
                },
                PlanetType.InfectedC2 => new DisplayPlanet
                {
                    Name = "Infect C.2",
                    Description = "Cluster Lvl 2 which has been infected by the Viral Race",
                    PopulationGrowth = "-35%",
                    AgricultureModifier = "0%",
                    ArtifactModifier = "-99%",
                    LandSize = "Based on planets clustered",
                    OreSize = "Based on planets clustered"
                },
                PlanetType.InfectedC3 => new DisplayPlanet
                {
                    Name = "Infect C.3",
                    Description = "Cluster Lvl 3 which has been infected by the Viral Race",
                    PopulationGrowth = "-40%",
                    AgricultureModifier = "+10%",
                    ArtifactModifier = "-99%",
                    LandSize = "Based on planets clustered",
                    OreSize = "Based on planets clustered"
                },
                PlanetType.ClusterLevel1 => new DisplayPlanet
                {
                    Name = "Colony Cluster Level 1",
                    Description = "5 planets clustered together as one colony. Acts as a planetary/colony upgrade and must be researched.",
                    PopulationGrowth = "+10%",
                    AgricultureModifier = "+15%",
                    ArtifactModifier = "-99%",
                    LandSize = "Based on planets clustered",
                    OreSize = "Based on planets clustered"
                },
                PlanetType.ClusterLevel2 => new DisplayPlanet
                {
                    Name = "Colony Cluster Level 2",
                    Description = "25 planets clustered together as one colony. Acts as a planetary/colony upgrade and must be researched.",
                    PopulationGrowth = "+20%",
                    AgricultureModifier = "+30%",
                    ArtifactModifier = "-99%",
                    LandSize = "Based on planets clustered",
                    OreSize = "Based on planets clustered"
                },
                PlanetType.ClusterLevel3 => new DisplayPlanet
                {
                    Name = "Colony Cluster Level 3",
                    Description = "125 planets clustered together as one colony. Acts as a planetary/colony upgrade and must be researched.",
                    PopulationGrowth = "+30%",
                    AgricultureModifier = "+45%",
                    ArtifactModifier = "-99%",
                    LandSize = "Based on planets clustered",
                    OreSize = "Based on planets clustered"
                },
                
                
            };
        }
    }
}
