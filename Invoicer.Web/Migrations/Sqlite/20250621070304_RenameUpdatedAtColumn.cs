using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Invoicer.Web.Migrations.Sqlite
{
    /// <inheritdoc />
    public partial class RenameUpdatedAtColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hours_Clients_ClientId",
                table: "Hours");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Clients_ClientId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_MyAccounts_AccountId",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "UpddatedAt",
                table: "Invoices",
                newName: "UpdatedAt");

            migrationBuilder.AlterColumn<string>(
                name: "CompanyName",
                table: "Clients",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Hours_Clients_ClientId",
                table: "Hours",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Clients_ClientId",
                table: "Invoices",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_MyAccounts_AccountId",
                table: "Invoices",
                column: "AccountId",
                principalTable: "MyAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hours_Clients_ClientId",
                table: "Hours");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Clients_ClientId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_MyAccounts_AccountId",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Invoices",
                newName: "UpddatedAt");

            migrationBuilder.AlterColumn<string>(
                name: "CompanyName",
                table: "Clients",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 100);

            migrationBuilder.AddForeignKey(
                name: "FK_Hours_Clients_ClientId",
                table: "Hours",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Clients_ClientId",
                table: "Invoices",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_MyAccounts_AccountId",
                table: "Invoices",
                column: "AccountId",
                principalTable: "MyAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
