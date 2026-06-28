using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SzyCo.Garage.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEventDefinitionsAndCarArchive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JsonDefinition",
                table: "EventTypeDefinitions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "{}");

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "Cars",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JsonDefinition",
                table: "EventTypeDefinitions");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "Cars");
        }
    }
}
