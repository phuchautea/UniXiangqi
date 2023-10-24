using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniXiangqi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalPointUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalPoint",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPoint",
                table: "AspNetUsers");
        }
    }
}
