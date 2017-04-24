using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KudosNetCore.Migrations
{
    public partial class AddForWhoToKudo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Kudos",
                newName: "ForWho");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Kudos",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Kudos");

            migrationBuilder.RenameColumn(
                name: "ForWho",
                table: "Kudos",
                newName: "UserName");
        }
    }
}
