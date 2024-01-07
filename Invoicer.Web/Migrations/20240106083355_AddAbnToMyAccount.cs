using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Invoicer.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddAbnToMyAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ABN",
                table: "MyAccounts",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ABN",
                table: "MyAccounts");
        }
    }
}
