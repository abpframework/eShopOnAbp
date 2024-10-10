using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShopOnAbp.CmskitService.Migrations
{
    /// <inheritdoc />
    public partial class Abp_8_3_1_Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "CmsComments",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "CmsComments");
        }
    }
}
