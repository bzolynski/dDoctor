using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistance.Migrations
{
    public partial class spec69 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Specializations_Code",
                table: "Specializations");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Specializations",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_Code",
                table: "Specializations",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Specializations_Code",
                table: "Specializations");

            migrationBuilder.AlterColumn<int>(
                name: "Code",
                table: "Specializations",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_Code",
                table: "Specializations",
                column: "Code",
                unique: true);
        }
    }
}
