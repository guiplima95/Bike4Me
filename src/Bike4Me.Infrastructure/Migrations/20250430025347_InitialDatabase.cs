using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bike4Me.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "bike4me");

            migrationBuilder.CreateTable(
                name: "bike_models",
                schema: "bike4me",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    engine_capacity = table.Column<string>(type: "text", nullable: false),
                    manufacturer = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    year = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bike_models", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "bikes",
                schema: "bike4me",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    license_plate = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    model_id = table.Column<Guid>(type: "uuid", nullable: false),
                    color = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bikes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "couriers",
                schema: "bike4me",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    cnh_number = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    cnh_category = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    cnpj = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_couriers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rentals",
                schema: "bike4me",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    motorcycle_id = table.Column<Guid>(type: "uuid", nullable: false),
                    courier_id = table.Column<Guid>(type: "uuid", nullable: false),
                    rental_start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    rental_end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expected_return_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    actual_return_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    total_price = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    rental_plan_additional_daily_fee = table.Column<decimal>(type: "numeric", nullable: false),
                    rental_plan_daily_rate = table.Column<decimal>(type: "numeric", nullable: false),
                    rental_plan_days = table.Column<int>(type: "integer", nullable: false),
                    rental_plan_penalty_percentage = table.Column<decimal>(type: "numeric", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_rentals", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "bike4me",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    role = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bikes_License_Plate_Unique",
                schema: "bike4me",
                table: "bikes",
                column: "license_plate",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bike_models",
                schema: "bike4me");

            migrationBuilder.DropTable(
                name: "bikes",
                schema: "bike4me");

            migrationBuilder.DropTable(
                name: "couriers",
                schema: "bike4me");

            migrationBuilder.DropTable(
                name: "rentals",
                schema: "bike4me");

            migrationBuilder.DropTable(
                name: "users",
                schema: "bike4me");
        }
    }
}
