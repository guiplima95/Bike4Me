using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bike4Me.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCnpjToCourier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "cnpj",
                schema: "bike4me",
                table: "couriers",
                type: "character varying(14)",
                maxLength: 14,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cnpj",
                schema: "bike4me",
                table: "couriers");
        }
    }
}