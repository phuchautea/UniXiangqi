using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniXiangqi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPieceMoveTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PieceMoves",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MatchId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PlayerUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MoveContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChessBoard = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PieceMoves", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PieceMoves_AspNetUsers_PlayerUserId",
                        column: x => x.PlayerUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PieceMoves_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PieceMoves_MatchId",
                table: "PieceMoves",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_PieceMoves_PlayerUserId",
                table: "PieceMoves",
                column: "PlayerUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PieceMoves");
        }
    }
}
