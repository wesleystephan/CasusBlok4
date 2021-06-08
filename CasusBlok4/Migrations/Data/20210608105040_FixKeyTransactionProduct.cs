using Microsoft.EntityFrameworkCore.Migrations;

namespace CasusBlok4.Migrations.Data
{
    public partial class FixKeyTransactionProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionProducts",
                table: "TransactionProducts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionProducts",
                table: "TransactionProducts",
                columns: new[] { "TransactionId", "ProductId", "IsForSell" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionProducts",
                table: "TransactionProducts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionProducts",
                table: "TransactionProducts",
                column: "TransactionId");
        }
    }
}
