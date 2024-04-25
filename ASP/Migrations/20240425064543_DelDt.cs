using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP.Migrations
{
    /// <inheritdoc />
    public partial class DelDt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDt",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDt",
                table: "Reservations",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedDt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DeletedDt",
                table: "Reservations");
        }
    }
}
