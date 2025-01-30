using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinkDev.Talabat.Infrastructure.Persistence._Identity.Migrations
{
    /// <inheritdoc />
    public partial class Add_EmailConfirmResetCode_And_EmailConfirmResetCodeExpiry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmailConfirmResetCode",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EmailConfirmResetCodeExpiry",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailConfirmResetCode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmailConfirmResetCodeExpiry",
                table: "AspNetUsers");
        }
    }
}
