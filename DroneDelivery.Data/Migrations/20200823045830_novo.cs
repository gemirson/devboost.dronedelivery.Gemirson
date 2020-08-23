using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DroneDelivery.Data.Migrations
{
    public partial class novo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Drones_DroneId",
                table: "Pedidos");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_DroneId",
                table: "Pedidos");

            migrationBuilder.AlterColumn<Guid>(
                name: "DroneId",
                table: "Pedidos",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_DroneId",
                table: "Pedidos",
                column: "DroneId",
                unique: true,
                filter: "[DroneId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Drones_DroneId",
                table: "Pedidos",
                column: "DroneId",
                principalTable: "Drones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Drones_DroneId",
                table: "Pedidos");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_DroneId",
                table: "Pedidos");

            migrationBuilder.AlterColumn<Guid>(
                name: "DroneId",
                table: "Pedidos",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_DroneId",
                table: "Pedidos",
                column: "DroneId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Drones_DroneId",
                table: "Pedidos",
                column: "DroneId",
                principalTable: "Drones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
