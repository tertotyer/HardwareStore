using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HardwareStore.Data.Migrations
{
    public partial class AddPriceThing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Thing",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Thing");
        }
    }
}
