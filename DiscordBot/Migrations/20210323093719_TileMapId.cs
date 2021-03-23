using Microsoft.EntityFrameworkCore.Migrations;

namespace DiscordBot.Migrations
{
    public partial class TileMapId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "graphic",
                table: "Tiles",
                newName: "Graphic");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Graphic",
                table: "Tiles",
                newName: "graphic");
        }
    }
}
