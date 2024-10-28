using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Barbearia.Migrations
{
    /// <inheritdoc />
    public partial class AllowMultipleExchangesForSameProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Remover a chave estrangeira existente
            migrationBuilder.DropForeignKey(
                name: "FK_Exchanges_Products_ProductId",
                table: "Exchanges");

            // 2. Remover o índice único existente
            migrationBuilder.DropIndex(
                name: "IX_Exchanges_ProductId",
                table: "Exchanges");

            // 3. Recriar o índice não único (se necessário)
            migrationBuilder.CreateIndex(
                name: "IX_Exchanges_ProductId",
                table: "Exchanges",
                column: "ProductId");

            // 4. Recriar a chave estrangeira sem a restrição de unicidade
            migrationBuilder.AddForeignKey(
                name: "FK_Exchanges_Products_ProductId",
                table: "Exchanges",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Restaurar o estado anterior em caso de rollback

            // 1. Remover a chave estrangeira atual
            migrationBuilder.DropForeignKey(
                name: "FK_Exchanges_Products_ProductId",
                table: "Exchanges");

            // 2. Remover o índice não único
            migrationBuilder.DropIndex(
                name: "IX_Exchanges_ProductId",
                table: "Exchanges");

            // 3. Recriar o índice único
            migrationBuilder.CreateIndex(
                name: "IX_Exchanges_ProductId",
                table: "Exchanges",
                column: "ProductId",
                unique: true);

            // 4. Recriar a chave estrangeira com o índice único
            migrationBuilder.AddForeignKey(
                name: "FK_Exchanges_Products_ProductId",
                table: "Exchanges",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
