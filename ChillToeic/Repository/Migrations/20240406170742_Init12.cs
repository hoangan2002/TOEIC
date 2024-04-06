using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChillToeic.Migrations
{
    public partial class Init12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestOfUsers_Courses_CourseId",
                table: "TestOfUsers");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "TestOfUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_TestOfUsers_Courses_CourseId",
                table: "TestOfUsers",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestOfUsers_Courses_CourseId",
                table: "TestOfUsers");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "TestOfUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TestOfUsers_Courses_CourseId",
                table: "TestOfUsers",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
