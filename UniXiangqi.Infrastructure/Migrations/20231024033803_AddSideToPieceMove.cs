using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniXiangqi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSideToPieceMove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Side",
                table: "PieceMoves",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Side",
                table: "PieceMoves");
        }
    }
}
