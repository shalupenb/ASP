using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP.Migrations
{
    /// <inheritdoc />
    public partial class Slugs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Rooms",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Locations",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Locations",
                newName: "adress");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Categories",
                newName: "id");

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "Rooms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Rooms",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Locations",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Categories",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_Slug",
                table: "Rooms",
                column: "Slug",
                unique: true,
                filter: "[Slug] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Slug",
                table: "Locations",
                column: "Slug",
                unique: true,
                filter: "[Slug] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Slug",
                table: "Categories",
                column: "Slug",
                unique: true,
                filter: "[Slug] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Rooms_Slug",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Locations_Slug",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Slug",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Rooms",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Locations",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "adress",
                table: "Locations",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Categories",
                newName: "Id");
        }
    }
}
