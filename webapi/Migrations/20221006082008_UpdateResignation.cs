using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    public partial class UpdateResignation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Resigned",
                table: "Users");

            migrationBuilder.AddColumn<DateTime>(
                name: "ResignedDate",
                table: "Users",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResignedDate",
                table: "Users");

            migrationBuilder.AddColumn<bool>(
                name: "Resigned",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
