using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library_Management_System.Migrations
{
    public partial class attrent : Migration
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
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: false);

            migrationBuilder.AddColumn<string>(
                name: "PersonId",
                table: "Rents",
                type: "nvarchar(11)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Rents_Properties_PropertiesId",
                table: "Rents",
                column: "PropertiesId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.DropForeignKey(
                name: "FK_Rents_Properties_PropertiesId",
                table: "Rents");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "Rents");

            migrationBuilder.AlterColumn<int>(
                name: "PropertiesId",
                table: "Rents",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "PersonCpf",
                table: "Rents",
                type: "nvarchar(11)",
                nullable: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Rents_Properties_PropertiesId",
                table: "Rents",
                column: "PropertiesId",
                principalTable: "Properties",
                principalColumn: "Id");
        }
    }
}
