using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bike4Me.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingUserPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "password_hash",
                schema: "bike4me",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "password_hash",
                schema: "bike4me",
                table: "users");
        }
    }
}
