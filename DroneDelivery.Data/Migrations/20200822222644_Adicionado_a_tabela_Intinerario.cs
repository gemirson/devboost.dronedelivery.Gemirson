using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DroneDelivery.Data.Migrations
{
    public partial class Adicionado_a_tabela_Intinerario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Pedidos_DroneId",
                table: "Pedidos");

            migrationBuilder.CreateTable(
                name: "Intinerarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IdDrone = table.Column<Guid>(nullable: false),
                    PesoAtual = table.Column<double>(nullable: false),
                    AutonomiaAtual = table.Column<double>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Intinerarios", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_DroneId",
                table: "Pedidos",
                column: "DroneId",
                unique: true,
                filter: "[DroneId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Intinerarios");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_DroneId",
                table: "Pedidos");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_DroneId",
                table: "Pedidos",
                column: "DroneId");
        }
    }
}
