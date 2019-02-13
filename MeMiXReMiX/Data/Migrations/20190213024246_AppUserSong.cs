using Microsoft.EntityFrameworkCore.Migrations;

namespace MeMiXReMiX.Data.Migrations
{
    public partial class AppUserSong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserID",
                table: "Songs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserUserNAme",
                table: "Songs",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Songs_ApplicationUserID",
                table: "Songs",
                column: "ApplicationUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_AspNetUsers_ApplicationUserID",
                table: "Songs",
                column: "ApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Songs_AspNetUsers_ApplicationUserID",
                table: "Songs");

            migrationBuilder.DropIndex(
                name: "IX_Songs_ApplicationUserID",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "ApplicationUserID",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "ApplicationUserUserNAme",
                table: "Songs");
        }
    }
}
