using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChillToeic.Migrations
{
    public partial class Init7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstablishmentDate",
                table: "EducationCenters");

            migrationBuilder.AlterColumn<string>(
                name: "Certification",
                table: "EducationCenters",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "EducationCenters",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "EducationCenters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EducationCenters_RoleId",
                table: "EducationCenters",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_EducationCenters_Roles_RoleId",
                table: "EducationCenters",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EducationCenters_Roles_RoleId",
                table: "EducationCenters");

            migrationBuilder.DropIndex(
                name: "IX_EducationCenters_RoleId",
                table: "EducationCenters");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "EducationCenters");

            migrationBuilder.AlterColumn<string>(
                name: "Certification",
                table: "EducationCenters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "EducationCenters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EstablishmentDate",
                table: "EducationCenters",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
