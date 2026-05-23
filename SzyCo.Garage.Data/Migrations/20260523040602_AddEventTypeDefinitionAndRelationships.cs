using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SzyCo.Garage.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEventTypeDefinitionAndRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventTypeDefinitions",
                columns: table => new
                {
                    EventTypeDefinitionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTypeDefinitions", x => x.EventTypeDefinitionId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_CarId",
                table: "Events",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_EventTypeId",
                table: "Events",
                column: "EventTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Cars_CarId",
                table: "Events",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "CarId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_EventTypeDefinitions_EventTypeId",
                table: "Events",
                column: "EventTypeId",
                principalTable: "EventTypeDefinitions",
                principalColumn: "EventTypeDefinitionId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Cars_CarId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_EventTypeDefinitions_EventTypeId",
                table: "Events");

            migrationBuilder.DropTable(
                name: "EventTypeDefinitions");

            migrationBuilder.DropIndex(
                name: "IX_Events_CarId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_EventTypeId",
                table: "Events");
        }
    }
}
