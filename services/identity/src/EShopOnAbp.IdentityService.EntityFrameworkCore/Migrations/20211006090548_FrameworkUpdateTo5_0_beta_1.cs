using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShopOnAbp.IdentityService.Migrations
{
    public partial class FrameworkUpdateTo5_0_beta_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AbpUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AbpUsers");
        }
    }
}
