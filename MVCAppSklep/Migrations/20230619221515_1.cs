using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCAppSklep.Migrations
{
    /// <inheritdoc />
    public partial class _1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produkty_Producenci_ProducentId",
                table: "Produkty");

            migrationBuilder.DropIndex(
                name: "IX_Produkty_ProducentId",
                table: "Produkty");

            migrationBuilder.DropColumn(
                name: "ProducentId",
                table: "Produkty");

            migrationBuilder.CreateIndex(
                name: "IX_Produkty_ProducerId",
                table: "Produkty",
                column: "ProducerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Produkty_Producenci_ProducerId",
                table: "Produkty",
                column: "ProducerId",
                principalTable: "Producenci",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produkty_Producenci_ProducerId",
                table: "Produkty");

            migrationBuilder.DropIndex(
                name: "IX_Produkty_ProducerId",
                table: "Produkty");

            migrationBuilder.AddColumn<int>(
                name: "ProducentId",
                table: "Produkty",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Produkty_ProducentId",
                table: "Produkty",
                column: "ProducentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Produkty_Producenci_ProducentId",
                table: "Produkty",
                column: "ProducentId",
                principalTable: "Producenci",
                principalColumn: "Id");
        }
    }
}
