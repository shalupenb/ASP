using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP.Migrations
{
    /// <inheritdoc />
    public partial class ResPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "OrderDt",
                table: "Reservations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Reservations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderDt",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Reservations");
        }
    }
}
