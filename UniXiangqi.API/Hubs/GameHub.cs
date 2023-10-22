using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using UniXiangqi.Application.Interfaces;
using UniXiangqi.Domain.Entities;
using UniXiangqi.Domain.Entities.Game;
using UniXiangqi.Infrastructure.Persistence;

namespace UniXiangqi.API.Hubs
{
    public class GameHub : Hub
    {
        public static Dictionary<string, Board> Boards { get; set; } = new();
        public static Dictionary<string, string> UserConnection { get; set; } = new Dictionary<string, string>();
        private IRoomService _roomService;
        private string roomCode;
        private IUserService _userService;
        private ApplicationDbContext _dbContext;
        public GameHub(IRoomService roomService, IUserService userService, ApplicationDbContext dbContext)
        {
            this._roomService = roomService;
            this._userService = userService;
            this._dbContext = dbContext;
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        //Thêm roomCode và JWT vào hàm sendMovePiece
        public async Task SendMovePiece(int fromRow, int fromCol, int toRow, int toCol)
        {
            await Clients.All.SendAsync("ReceiveMovePiece", fromRow, fromCol, toRow, toCol);
        }
        public async Task SendMessageToUser(string userId, string message)
        {
            if (UserConnection.TryGetValue(userId, out var connectionId))
            {
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", "SEND PRIVATE MESSAGE", message);
            }
            else
            {
                // Người dùng không online hoặc ConnectionId không tồn tại
            }
        }
        public async Task AddConnection(string userId, string connectionId, string roomCode)
        {
            if (!UserConnection.ContainsKey(userId))
            {
                UserConnection.Add(userId, connectionId);
                await Groups.AddToGroupAsync(connectionId, roomCode);
            }
        }
        public async Task<string> GetConnectionId(string userId)
        {
            if (UserConnection.TryGetValue(userId, out string connectionId))
            {
                return connectionId;
            }
            return null;
        }
        public async Task SendRoomCode(string roomCode, string jwt)
        {
            this.roomCode = roomCode;
            Context.GetHttpContext().Items["jwt"] = jwt;

            var room = await _dbContext.Rooms.FirstOrDefaultAsync(x => x.Code == roomCode);

            if (room == null)
            {
                Context.Abort();
                return;
            }
            var infoUser = await _userService.GetUserInfo();
            if (infoUser.statusCode == 1)
            {
                if (infoUser.data.Id == room.HostUserId)
                {
                    //Chủ phòng
                    await AddConnection(infoUser.data.Id, Context.ConnectionId, roomCode);
                }
                else
                {
                    // Đối thủ
                    // Kiểm tra phòng đã có đối thủ chưa
                    if (room.OpponentUserId != null)
                    {
                        // Kiểm tra đối thủ có phải là mình không
                        if (room.OpponentUserId == infoUser.data.Id)
                        {
                            await AddConnection(infoUser.data.Id, Context.ConnectionId, roomCode);
                        }
                        else
                        {
                            // Join vào làm viewers
                            var existingEntity = _dbContext.UserInRooms.FirstOrDefault(x => x.UserId == infoUser.data.Id &&
                                                                                        x.Room.Code == roomCode);

                            if (existingEntity == null)
                            {
                                _dbContext.UserInRooms.Add(new UserInRoom
                                {
                                    UserId = infoUser.data.Id,
                                    Room = room,
                                    IsPlayer = false
                                });
                            }
                            _dbContext.SaveChanges();
                        }
                    }
                    else
                    {
                        // Chưa có đối thủ

                        room.OpponentUserId = infoUser.data.Id;
                        await _dbContext.SaveChangesAsync();

                        await AddConnection(infoUser.data.Id, Context.ConnectionId, roomCode);
                    }
                }


                //await _roomService.Join(new JoinRoomDto
                //{
                //    RoomCode = roomCode,
                //    UserId = infoUser.data.Id
                //});
                if (UserConnection.TryGetValue(infoUser.data.Id, out var connectionId))
                {
                    await Clients.Client(connectionId).SendAsync("ReceiveMessage", "SEND PRIVATE MESSAGE", "Ở trong nhóm private");
                }
                else
                {
                    // Người dùng không online hoặc ConnectionId không tồn tại
                }
                Debug.WriteLine($"{infoUser.data.UserName} | {Context.ConnectionId} connected hub");
            }

            var hasRoom = Boards.TryGetValue(roomCode, out var board);
            if (!hasRoom)
            {
                var boardData = "RX,RM,RT,RS,RV,RS,RT,RM,RX,0,0,0,0,0,0,0,0,0,0,RP,0,0,0,0,0,RP,0,RC,0,RC,0,RC,0,RC,0,RC,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,BC,0,BC,0,BC,0,BC,0,BC,0,BP,0,0,0,0,0,BP,0,0,0,0,0,0,0,0,0,0,BX,BM,BT,BS,BV,BS,BT,BM,BX";
                Boards.Add(roomCode, new Board(boardData));

                board = Boards[roomCode];
            }
            
            if (board == null)
            {
                Context.Abort();
                return;
            }
        }
        public override async Task OnConnectedAsync()
        {
            
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {

        }
    }
}
