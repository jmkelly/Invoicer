using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Invoicer.Web.Migrations
{
	/// <inheritdoc />
	public partial class RenameWorkItemsToHours : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.RenameTable(
				name: "WorkItems",
				newName: "Hours");

			migrationBuilder.RenameColumn(
					name: "Hours",
					table: "Hours",
					newName: "NumberOfHours");

		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.RenameTable(
				name: "Hours",
				newName: "WorkItems");

			migrationBuilder.RenameColumn(
					name: "NumberOfHours",
					table: "WorkItems",
					newName: "Hours");
		}
	}
}
