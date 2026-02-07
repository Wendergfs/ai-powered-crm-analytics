using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIClientManager.Migrations
{
    /// <inheritdoc />
    public partial class AddAIFieldsToClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AnalyzedAt",
                table: "Clients",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Keywords",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Score",
                table: "Clients",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnalyzedAt",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Keywords",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Summary",
                table: "Clients");
        }
    }
}
