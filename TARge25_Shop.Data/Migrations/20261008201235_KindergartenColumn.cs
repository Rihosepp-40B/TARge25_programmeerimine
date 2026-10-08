using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TARge25_Shop.Data.Migrations
{
    /// <inheritdoc />
    public partial class KindergartenColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "KindergartenId",
                table: "FileToDatabases",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Down meetod eemaldab vajadusel ainult selle uue veeru:
            migrationBuilder.DropColumn(
                name: "KindergartenId",
                table: "FileToDatabases");
        }
    }
}
