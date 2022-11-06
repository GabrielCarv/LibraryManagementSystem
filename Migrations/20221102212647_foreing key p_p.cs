using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library_Management_System.Migrations
{
    public partial class foreingkeyp_p : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rents_Properties_PropertiesId",
                table: "Rents");

            migrationBuilder.AlterColumn<int>(
                name: "PropertiesId",
                table: "Rents",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "IdPerson",
                table: "Phones",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Rents_Properties_PropertiesId",
                table: "Rents",
                column: "PropertiesId",
                principalTable: "Properties",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rents_Properties_PropertiesId",
                table: "Rents");

            migrationBuilder.DropColumn(
                name: "Info",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "IdPerson",
                table: "Phones");

            migrationBuilder.AlterColumn<int>(
                name: "PropertiesId",
                table: "Rents",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Rents_Properties_PropertiesId",
                table: "Rents",
                column: "PropertiesId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
