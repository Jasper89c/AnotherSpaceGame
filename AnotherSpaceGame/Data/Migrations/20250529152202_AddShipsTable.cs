using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnotherSpaceGame.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddShipsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CyrilAlloy",
                table: "CyrilClassResearches",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CyrilAlloyTurnsRequired",
                table: "CyrilClassResearches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AMinerResearches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StarshipEngineering = table.Column<bool>(type: "bit", nullable: false),
                    StarshipEngineeringTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    FlagshipEngineering = table.Column<bool>(type: "bit", nullable: false),
                    FlagshipEngineeringTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    AdvancedEngineering = table.Column<bool>(type: "bit", nullable: false),
                    AdvancedEngineeringTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    SensorEngineering = table.Column<bool>(type: "bit", nullable: false),
                    SensorEngineeringTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    StarbaseEngineering = table.Column<bool>(type: "bit", nullable: false),
                    StarbaseEngineeringTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    AsphaCorvette = table.Column<bool>(type: "bit", nullable: false),
                    AsphaCorvetteTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    AsphaFrigate = table.Column<bool>(type: "bit", nullable: false),
                    AsphaFrigateTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    AsphaDestroyer = table.Column<bool>(type: "bit", nullable: false),
                    AsphaDestroyerTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    AsphaCruiser = table.Column<bool>(type: "bit", nullable: false),
                    AsphaCruiserTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    AsphaBattleship = table.Column<bool>(type: "bit", nullable: false),
                    AsphaBattleshipTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    AsphaDreadnought = table.Column<bool>(type: "bit", nullable: false),
                    AsphaDreadnoughtTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    AsphaSeeker = table.Column<bool>(type: "bit", nullable: false),
                    AsphaSeekerTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    AsphaRanger = table.Column<bool>(type: "bit", nullable: false),
                    AsphaRangerTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    AsphaLightStarbase = table.Column<bool>(type: "bit", nullable: false),
                    AsphaLightStarbaseTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    AsphaHeavyStarbase = table.Column<bool>(type: "bit", nullable: false),
                    AsphaHeavyStarbaseTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    GClassShip = table.Column<bool>(type: "bit", nullable: false),
                    GClassShipTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    GClassJuggernaught = table.Column<bool>(type: "bit", nullable: false),
                    GClassJuggernaughtTurnsRequired = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AMinerResearches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AMinerResearches_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClusterResearches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ColonyClusterLevel1 = table.Column<bool>(type: "bit", nullable: false),
                    ColonyClusterLevel1TurnsRequired = table.Column<int>(type: "int", nullable: false),
                    ColonyClusterLevel2 = table.Column<bool>(type: "bit", nullable: false),
                    ColonyClusterLevel2TurnsRequired = table.Column<int>(type: "int", nullable: false),
                    ColonyClusterLevel3 = table.Column<bool>(type: "bit", nullable: false),
                    ColonyClusterLevel3TurnsRequired = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClusterResearches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClusterResearches_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CollectiveResearches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FrigateClass = table.Column<bool>(type: "bit", nullable: false),
                    FrigateClassTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    DestroyerClass = table.Column<bool>(type: "bit", nullable: false),
                    DestroyerClassTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    CruiserClass = table.Column<bool>(type: "bit", nullable: false),
                    CruiserClassTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    HClassShips = table.Column<bool>(type: "bit", nullable: false),
                    HClassShipsTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    RClassFrigate = table.Column<bool>(type: "bit", nullable: false),
                    RClassFrigateTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    HClassDestroyer = table.Column<bool>(type: "bit", nullable: false),
                    HClassDestroyerTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    RClassDestroyer = table.Column<bool>(type: "bit", nullable: false),
                    RClassDestroyerTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    HClassCruiser = table.Column<bool>(type: "bit", nullable: false),
                    HClassCruiserTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    RClassBattleship = table.Column<bool>(type: "bit", nullable: false),
                    RClassBattleshipTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    HClassDreadnought = table.Column<bool>(type: "bit", nullable: false),
                    HClassDreadnoughtTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    RClassDreadnought = table.Column<bool>(type: "bit", nullable: false),
                    RClassDreadnoughtTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    RClassJuggernaught = table.Column<bool>(type: "bit", nullable: false),
                    RClassJuggernaughtTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    HClassLeviathan = table.Column<bool>(type: "bit", nullable: false),
                    HClassLeviathanTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    HClassStarbase = table.Column<bool>(type: "bit", nullable: false),
                    HClassStarbaseTurnsRequired = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectiveResearches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CollectiveResearches_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CollectiveSpecificResearches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CollectiveClustering = table.Column<bool>(type: "bit", nullable: false),
                    CollectiveClusteringTurnsRequired = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectiveSpecificResearches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CollectiveSpecificResearches_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FClassResearches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FClassEngineering = table.Column<bool>(type: "bit", nullable: false),
                    FClassEngineeringTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    FClassFrigate = table.Column<bool>(type: "bit", nullable: false),
                    FClassFrigateTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    FClassDestroyer = table.Column<bool>(type: "bit", nullable: false),
                    FClassDestroyerTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    FClassCruiser = table.Column<bool>(type: "bit", nullable: false),
                    FClassCruiserTurnsRequired = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FClassResearches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FClassResearches_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GuardianResearches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FighterClass = table.Column<bool>(type: "bit", nullable: false),
                    FighterClassTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    CorvetteClass = table.Column<bool>(type: "bit", nullable: false),
                    CorvetteClassTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    FrigateClass = table.Column<bool>(type: "bit", nullable: false),
                    FrigateClassTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    DestroyerClass = table.Column<bool>(type: "bit", nullable: false),
                    DestroyerClassTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    CruiserClass = table.Column<bool>(type: "bit", nullable: false),
                    CruiserClassTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    WeaponsPlatform = table.Column<bool>(type: "bit", nullable: false),
                    WeaponsPlatformTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    AdvancedScouts = table.Column<bool>(type: "bit", nullable: false),
                    AdvancedScoutsTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    LClassFrigate = table.Column<bool>(type: "bit", nullable: false),
                    LClassFrigateTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    LClassDestroyer = table.Column<bool>(type: "bit", nullable: false),
                    LClassDestroyerTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    LClassCruiser = table.Column<bool>(type: "bit", nullable: false),
                    LClassCruiserTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    KClassDestroyer = table.Column<bool>(type: "bit", nullable: false),
                    KClassDestroyerTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    KClassCruiser = table.Column<bool>(type: "bit", nullable: false),
                    KClassCruiserTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    AdvancedEngineering = table.Column<bool>(type: "bit", nullable: false),
                    AdvancedEngineeringTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    FocusCrystal = table.Column<bool>(type: "bit", nullable: false),
                    FocusCrystalTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    OrganicSensors = table.Column<bool>(type: "bit", nullable: false),
                    OrganicSensorsTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    LanceTorpodoes = table.Column<bool>(type: "bit", nullable: false),
                    LanceTorpodoesTurnsRequired = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuardianResearches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuardianResearches_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MarauderResearches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Class3MarauderHull = table.Column<bool>(type: "bit", nullable: false),
                    Class3MarauderHullTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    Class4MarauderHull = table.Column<bool>(type: "bit", nullable: false),
                    Class4MarauderHullTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    Class5MarauderHull = table.Column<bool>(type: "bit", nullable: false),
                    Class5MarauderHullTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    Class6MarauderHull = table.Column<bool>(type: "bit", nullable: false),
                    Class6MarauderHullTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    AdvancedEngineering = table.Column<bool>(type: "bit", nullable: false),
                    AdvancedEngineeringTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    MarauderCorvette = table.Column<bool>(type: "bit", nullable: false),
                    MarauderCorvetteTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    MarauderFrigate = table.Column<bool>(type: "bit", nullable: false),
                    MarauderFrigateTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    MarauderDestroyer = table.Column<bool>(type: "bit", nullable: false),
                    MarauderDestroyerTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    MarauderCruiser = table.Column<bool>(type: "bit", nullable: false),
                    MarauderCruiserTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    MarauderBattleship = table.Column<bool>(type: "bit", nullable: false),
                    MarauderBattleshipTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    TypeDFrigate = table.Column<bool>(type: "bit", nullable: false),
                    TypeDFrigateTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    TypeDDestroyer = table.Column<bool>(type: "bit", nullable: false),
                    TypeDDestroyerTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    TypeDCruiser = table.Column<bool>(type: "bit", nullable: false),
                    TypeDCruiserTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    TypeDBattleship = table.Column<bool>(type: "bit", nullable: false),
                    TypeDBattleshipTurnsRequired = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarauderResearches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarauderResearches_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectsResearches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CapsuleLaboratory = table.Column<bool>(type: "bit", nullable: false),
                    CapsuleLaboratoryTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    KalZulOvermind = table.Column<bool>(type: "bit", nullable: false),
                    KalZulOvermindTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    KalZulArtifact = table.Column<bool>(type: "bit", nullable: false),
                    KalZulArtifactTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    ITechLaboratory = table.Column<bool>(type: "bit", nullable: false),
                    ITechLaboratoryTurnsRequired = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectsResearches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectsResearches_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShipName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShipType = table.Column<int>(type: "int", nullable: false),
                    PowerRating = table.Column<int>(type: "int", nullable: false),
                    Range = table.Column<int>(type: "int", nullable: false),
                    Weapon = table.Column<int>(type: "int", nullable: false),
                    Hull = table.Column<int>(type: "int", nullable: false),
                    EnergyWeapon = table.Column<int>(type: "int", nullable: false),
                    KineticWeapon = table.Column<int>(type: "int", nullable: false),
                    MissileWeapon = table.Column<int>(type: "int", nullable: false),
                    ChemicalWeapon = table.Column<int>(type: "int", nullable: false),
                    EnergyShield = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    KineticShield = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MissileShield = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ChemicalShield = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NoDefense = table.Column<bool>(type: "bit", nullable: false),
                    NoRetal = table.Column<bool>(type: "bit", nullable: false),
                    CapChance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cost = table.Column<int>(type: "int", nullable: false),
                    Upkeep = table.Column<int>(type: "int", nullable: false),
                    TerranMetal = table.Column<int>(type: "int", nullable: false),
                    RedCrystal = table.Column<int>(type: "int", nullable: false),
                    WhiteCrystal = table.Column<int>(type: "int", nullable: false),
                    Rutile = table.Column<int>(type: "int", nullable: false),
                    Composite = table.Column<int>(type: "int", nullable: false),
                    StrafezOrganism = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ships", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StrafezResearches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BasicGeneticEngineering = table.Column<bool>(type: "bit", nullable: false),
                    BasicGeneticEngineeringTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    SmallGeneticEngineering = table.Column<bool>(type: "bit", nullable: false),
                    SmallGeneticEngineeringTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    LargeGeneticEngineering = table.Column<bool>(type: "bit", nullable: false),
                    LargeGeneticEngineeringTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    SmallStrafezRunnerFodder = table.Column<bool>(type: "bit", nullable: false),
                    SmallStrafezRunnerFodderTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    LargeStrafezRunnerFodder = table.Column<bool>(type: "bit", nullable: false),
                    LargeStrafezRunnerFodderTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    StrafezQueenKing = table.Column<bool>(type: "bit", nullable: false),
                    StrafezQueenKingTurnsRequired = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StrafezResearches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StrafezResearches_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TerranResearches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SmallTerranHull = table.Column<bool>(type: "bit", nullable: false),
                    SmallTerranHullTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    MediumTerranHull = table.Column<bool>(type: "bit", nullable: false),
                    MediumTerranHullTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    LargeTerranHull = table.Column<bool>(type: "bit", nullable: false),
                    LargeTerranHullTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    TerranCorvette = table.Column<bool>(type: "bit", nullable: false),
                    TerranCorvetteTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    TerranFrigate = table.Column<bool>(type: "bit", nullable: false),
                    TerranFrigateTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    TerranDestroyer = table.Column<bool>(type: "bit", nullable: false),
                    TerranDestroyerTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    TerranCruiser = table.Column<bool>(type: "bit", nullable: false),
                    TerranCruiserTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    TerranBattleship = table.Column<bool>(type: "bit", nullable: false),
                    TerranBattleshipTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    TerranAdvancedScout = table.Column<bool>(type: "bit", nullable: false),
                    TerranAdvancedScoutTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    TerranDreadnaught = table.Column<bool>(type: "bit", nullable: false),
                    TerranDreadnaughtTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    TerranStarbase = table.Column<bool>(type: "bit", nullable: false),
                    TerranStarbaseTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    TerranJuggernaught = table.Column<bool>(type: "bit", nullable: false),
                    TerranJuggernaughtTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    AdvancedRadar = table.Column<bool>(type: "bit", nullable: false),
                    AdvancedRadarTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    CrystalLenses = table.Column<bool>(type: "bit", nullable: false),
                    CrystalLensesTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    PhotonTorpedo = table.Column<bool>(type: "bit", nullable: false),
                    PhotonTorpedoTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    PhotonCorvette = table.Column<bool>(type: "bit", nullable: false),
                    PhotonCorvetteTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    PhotonFrigate = table.Column<bool>(type: "bit", nullable: false),
                    PhotonFrigateTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    PhotonDestroyer = table.Column<bool>(type: "bit", nullable: false),
                    PhotonDestroyerTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    PhotonCruiser = table.Column<bool>(type: "bit", nullable: false),
                    PhotonCruiserTurnsRequired = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TerranResearches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TerranResearches_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ViralResearches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Class1ViralHull = table.Column<bool>(type: "bit", nullable: false),
                    Class1ViralHullTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    Class2ViralHull = table.Column<bool>(type: "bit", nullable: false),
                    Class2ViralHullTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    AdvancedExploration = table.Column<bool>(type: "bit", nullable: false),
                    AdvancedExplorationTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    KohoutekScout = table.Column<bool>(type: "bit", nullable: false),
                    KohoutekScoutTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    VClassDestroyer = table.Column<bool>(type: "bit", nullable: false),
                    VClassDestroyerTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    VClassCruiser = table.Column<bool>(type: "bit", nullable: false),
                    VClassCruiser1TurnsRequired = table.Column<int>(type: "int", nullable: false),
                    BClassCruiser = table.Column<bool>(type: "bit", nullable: false),
                    BClassCruiserTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    BClassStarbase = table.Column<bool>(type: "bit", nullable: false),
                    BClassStarbaseTurnsRequired = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViralResearches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ViralResearches_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ViralSpecificResearches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ViralClustering = table.Column<bool>(type: "bit", nullable: false),
                    ViralClusteringTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    UnreverseEngineering = table.Column<bool>(type: "bit", nullable: false),
                    UnreverseEngineeringTurnsRequired = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViralSpecificResearches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ViralSpecificResearches_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AMinerResearches_ApplicationUserId",
                table: "AMinerResearches",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClusterResearches_ApplicationUserId",
                table: "ClusterResearches",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CollectiveResearches_ApplicationUserId",
                table: "CollectiveResearches",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CollectiveSpecificResearches_ApplicationUserId",
                table: "CollectiveSpecificResearches",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FClassResearches_ApplicationUserId",
                table: "FClassResearches",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GuardianResearches_ApplicationUserId",
                table: "GuardianResearches",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MarauderResearches_ApplicationUserId",
                table: "MarauderResearches",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectsResearches_ApplicationUserId",
                table: "ProjectsResearches",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StrafezResearches_ApplicationUserId",
                table: "StrafezResearches",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TerranResearches_ApplicationUserId",
                table: "TerranResearches",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ViralResearches_ApplicationUserId",
                table: "ViralResearches",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ViralSpecificResearches_ApplicationUserId",
                table: "ViralSpecificResearches",
                column: "ApplicationUserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AMinerResearches");

            migrationBuilder.DropTable(
                name: "ClusterResearches");

            migrationBuilder.DropTable(
                name: "CollectiveResearches");

            migrationBuilder.DropTable(
                name: "CollectiveSpecificResearches");

            migrationBuilder.DropTable(
                name: "FClassResearches");

            migrationBuilder.DropTable(
                name: "GuardianResearches");

            migrationBuilder.DropTable(
                name: "MarauderResearches");

            migrationBuilder.DropTable(
                name: "ProjectsResearches");

            migrationBuilder.DropTable(
                name: "Ships");

            migrationBuilder.DropTable(
                name: "StrafezResearches");

            migrationBuilder.DropTable(
                name: "TerranResearches");

            migrationBuilder.DropTable(
                name: "ViralResearches");

            migrationBuilder.DropTable(
                name: "ViralSpecificResearches");

            migrationBuilder.DropColumn(
                name: "CyrilAlloy",
                table: "CyrilClassResearches");

            migrationBuilder.DropColumn(
                name: "CyrilAlloyTurnsRequired",
                table: "CyrilClassResearches");
        }
    }
}
