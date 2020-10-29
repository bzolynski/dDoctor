using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistance.Migrations
{
    public partial class DoctorsMigration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Specializations_SpecializationId",
                table: "Schedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Specializations",
                table: "Specializations");

            migrationBuilder.DropColumn(
                name: "CodeId",
                table: "Specializations");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Specializations",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Code",
                table: "Specializations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Specializations",
                table: "Specializations",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_Code",
                table: "Specializations",
                column: "Code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Specializations_SpecializationId",
                table: "Schedules",
                column: "SpecializationId",
                principalTable: "Specializations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Specializations_SpecializationId",
                table: "Schedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Specializations",
                table: "Specializations");

            migrationBuilder.DropIndex(
                name: "IX_Specializations_Code",
                table: "Specializations");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Specializations");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Specializations");

            migrationBuilder.AddColumn<int>(
                name: "CodeId",
                table: "Specializations",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Specializations",
                table: "Specializations",
                column: "CodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Specializations_SpecializationId",
                table: "Schedules",
                column: "SpecializationId",
                principalTable: "Specializations",
                principalColumn: "CodeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
