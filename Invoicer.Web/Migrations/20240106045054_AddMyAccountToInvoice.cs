using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Invoicer.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddMyAccountToInvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "Invoices",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_AccountId",
                table: "Invoices",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_MyAccounts_AccountId",
                table: "Invoices",
                column: "AccountId",
                principalTable: "MyAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_MyAccounts_AccountId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_AccountId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Invoices");
        }
    }
}
