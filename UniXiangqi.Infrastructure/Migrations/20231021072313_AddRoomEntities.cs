using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniXiangqi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRoomEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GameTimer = table.Column<int>(type: "int", nullable: false),
                    MoveTimer = table.Column<int>(type: "int", nullable: false),
                    HostUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    OpponentUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    HostSide = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRated = table.Column<bool>(type: "bit", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalUser = table.Column<int>(type: "int", nullable: false),
                    IsRedTurn = table.Column<bool>(type: "bit", nullable: false),
                    IsHostTurn = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_AspNetUsers_HostUserId",
                        column: x => x.HostUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rooms_AspNetUsers_OpponentUserId",
                        column: x => x.OpponentUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoomId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RedUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BlackUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WinnerUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Turn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NextTurn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MatchStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserInRooms",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RoomId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsPlayer = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserInRooms_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserInRooms_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_RoomId",
                table: "Matches",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_HostUserId",
                table: "Rooms",
                column: "HostUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_OpponentUserId",
                table: "Rooms",
                column: "OpponentUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInRooms_RoomId",
                table: "UserInRooms",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInRooms_UserId",
                table: "UserInRooms",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "UserInRooms");

            migrationBuilder.DropTable(
                name: "Rooms");
        }
    }
}
