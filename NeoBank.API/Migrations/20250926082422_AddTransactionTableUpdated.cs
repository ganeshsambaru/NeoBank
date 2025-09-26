using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeoBank.API.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionTableUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Customers_CustomerId1",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_CustomerId1",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "CustomerId1",
                table: "Accounts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerId1",
                table: "Accounts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_CustomerId1",
                table: "Accounts",
                column: "CustomerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Customers_CustomerId1",
                table: "Accounts",
                column: "CustomerId1",
                principalTable: "Customers",
                principalColumn: "Id");
        }
    }
}
