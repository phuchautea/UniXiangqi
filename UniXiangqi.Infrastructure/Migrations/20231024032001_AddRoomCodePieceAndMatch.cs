using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniXiangqi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRoomCodePieceAndMatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RoomCode",
                table: "PieceMoves",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoomCode",
                table: "Matches",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoomCode",
                table: "PieceMoves");

            migrationBuilder.DropColumn(
                name: "RoomCode",
                table: "Matches");
        }
    }
}
