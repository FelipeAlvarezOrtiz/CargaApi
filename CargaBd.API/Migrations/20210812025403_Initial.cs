using Microsoft.EntityFrameworkCore.Migrations;

namespace CargaBd.API.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Payload",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    TrackingId = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
                    Latitude = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Longitude = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Load = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Load2 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Load3 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WindowStart = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WindowEnd = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WindowStart2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WindowEnd2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Duration = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContactName = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
                    ContactPhone = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PlannedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProgrammedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Route = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    RouteEstimatedTimeStart = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstimatedTimeArrival = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstimatedTimeDeparture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckinTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckoutTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckoutLatitude = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckoutLongitude = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckoutCommet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckoutObservation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Signature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modified = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EtaPredicted = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EtaCurrent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Driver = table.Column<int>(type: "int", nullable: false),
                    Vehicle = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<bool>(type: "bit", nullable: false),
                    Has_alert = table.Column<bool>(type: "bit", nullable: false),
                    PriorityLevel = table.Column<int>(type: "int", nullable: false),
                    ExtraFieldValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeocodeAlert = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VisitType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentEta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fleet = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payload", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pictures",
                columns: table => new
                {
                    IdPicture = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UrlPicture = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PayloadId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pictures", x => x.IdPicture);
                    table.ForeignKey(
                        name: "FK_Pictures_Payload_PayloadId",
                        column: x => x.PayloadId,
                        principalTable: "Payload",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SkillsOptionals",
                columns: table => new
                {
                    IdSkill = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreSkillOptional = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    PayloadId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillsOptionals", x => x.IdSkill);
                    table.ForeignKey(
                        name: "FK_SkillsOptionals_Payload_PayloadId",
                        column: x => x.PayloadId,
                        principalTable: "Payload",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SkillsRequired",
                columns: table => new
                {
                    IdSkill = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreSkillRequired = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    PayloadId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillsRequired", x => x.IdSkill);
                    table.ForeignKey(
                        name: "FK_SkillsRequired_Payload_PayloadId",
                        column: x => x.PayloadId,
                        principalTable: "Payload",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    IdTag = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreTag = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    PayloadId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.IdTag);
                    table.ForeignKey(
                        name: "FK_Tags_Payload_PayloadId",
                        column: x => x.PayloadId,
                        principalTable: "Payload",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_PayloadId",
                table: "Pictures",
                column: "PayloadId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillsOptionals_PayloadId",
                table: "SkillsOptionals",
                column: "PayloadId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillsRequired_PayloadId",
                table: "SkillsRequired",
                column: "PayloadId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_PayloadId",
                table: "Tags",
                column: "PayloadId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pictures");

            migrationBuilder.DropTable(
                name: "SkillsOptionals");

            migrationBuilder.DropTable(
                name: "SkillsRequired");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Payload");
        }
    }
}
