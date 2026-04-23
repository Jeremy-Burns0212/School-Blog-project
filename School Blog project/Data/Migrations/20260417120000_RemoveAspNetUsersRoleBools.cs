using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School_Blog_project.Data.Migrations
{
	public partial class RemoveAspNetUsersRoleBools : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "IsEditor",
				table: "AspNetUsers");

			migrationBuilder.DropColumn(
				name: "IsWriter",
				table: "AspNetUsers");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<bool>(
				name: "IsEditor",
				table: "AspNetUsers",
				type: "bit",
				nullable: false,
				defaultValue: false);

			migrationBuilder.AddColumn<bool>(
				name: "IsWriter",
				table: "AspNetUsers",
				type: "bit",
				nullable: false,
				defaultValue: false);
		}
	}
}
