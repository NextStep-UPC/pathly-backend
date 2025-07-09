using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pathly_backend.Sessions.Migrations
{
    /// <inheritdoc />
    public partial class AddAssignedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AssignedAtUtc",
                table: "Sessions",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedAtUtc",
                table: "Sessions");
        }
    }
}
