using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Diagnostics;
using System.Text.RegularExpressions;
using UniXiangqi.Application.DTOs.Match;
using UniXiangqi.Application.DTOs.User;
using UniXiangqi.Application.Interfaces;
using UniXiangqi.Domain.Entities;
using UniXiangqi.Domain.Entities.Game;
using UniXiangqi.Infrastructure.Persistence;
using UniXiangqi.Infrastructure.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UniXiangqi.API.Hubs
{
    public class GameHub : Hub
    {
        public static Dictionary<string, Board> Boards { get; set; } = new();
        public static Dictionary<string, string> UserConnection { get; set; } = new Dictionary<string, string>();
        private IRoomService _roomService;
        private IUserService _userService;
        private IPieceMoveService _pieceMoveService;
        private IMatchService _matchService;
        private ApplicationDbContext _dbContext;
        public GameHub(IRoomService roomService, IUserService userService, IMatchService matchService, IPieceMoveService pieceMoveService, ApplicationDbContext dbContext)
        {
            this._roomService = roomService;
            this._userService = userService;
            this._matchService = matchService;
            this._pieceMoveService = pieceMoveService;
            this._dbContext = dbContext;
        }
        public async Task CreateGame(string roomCode, InfoResponse infoUser)
        {
            var room = _dbContext.Rooms.SingleOrDefault(x => x.Code == roomCode);
            if (room != null && infoUser != null)
            {
                // Kiểm tra xem có HostUserId với UserId trong Groups SignalR không
                if (UserConnection.TryGetValue(room.HostUserId, out var HostUserConnectionId) &&
                    UserConnection.TryGetValue(room.OpponentUserId, out var OpponentUserConnectionId))
                {
                    var createMatch = await _matchService.Create(new CreateMatchRequest
                    {
                        roomCode = roomCode
                    });
                    var match = _dbContext.Matches.SingleOrDefault(x => x.Id == createMatch.MatchId);
                    // Tạo bàn cờ mặc định
                    var boardData = "RX,RM,RT,RS,RV,RS,RT,RM,RX,0,0,0,0,0,0,0,0,0,0,RP,0,0,0,0,0,RP,0,RC,0,RC,0,RC,0,RC,0,RC,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,BC,0,BC,0,BC,0,BC,0,BC,0,BP,0,0,0,0,0,BP,0,0,0,0,0,0,0,0,0,0,BX,BM,BT,BS,BV,BS,BT,BM,BX";
                    await _pieceMoveService.Create(new Application.DTOs.PieceMove.CreatePieceMoveDto
                    {
                        MatchId = match.Id,
                        PlayerUserId = infoUser.Id,
                        MoveContent = ConvertToChessNotation(0, 0, 0, 0, "X"),
                        ChessBoard = boardData,
                        RoomCode = roomCode,
                        Side = match.BlackUserId == infoUser.Id ? "B" : "R",
                    });
                    await Clients.Group(roomCode).SendAsync("ReceiveSuccessAlert", "Tạo trận đấu thành công, Gooooo!!!");
                    await Clients.Group(roomCode).SendAsync("ReceiveBoard", boardData);
                }
                else
                {
                    await Clients.Group(roomCode).SendAsync("ReceiveErrorAlert", "Chưa đủ người chơi, không thể tạo game");
                }
            }
        }

        public async Task LoadGame(string roomCode, InfoResponse infoUser)
        {
            var room = _dbContext.Rooms.SingleOrDefault(x => x.Code == roomCode);
            if (room != null)
            {
                // Kiểm tra xem có HostUserId với UserId trong Groups SignalR không
                if (UserConnection.TryGetValue(room.HostUserId, out var HostUserConnectionId) &&
                    UserConnection.TryGetValue(room.OpponentUserId, out var OpponentUserConnectionId))
                {
                    //var match = await _dbContext.Matches.FirstOrDefaultAsync(x => x.RoomId == room.Id);
                    //var getInfoMatch = await _matchService.GetInfoMatch(match.Id);

                    //infoMatches.Add(roomCode, match);
                    //await Clients.Group(roomCode).SendAsync("ReceiveMessage", "SYSTEM", "" + infoUser.data.UserName + " đã kết nối tới phòng");
                    //await Clients.Group(roomCode).SendAsync("ReceiveMatchId", match.Id);
                    var getLastestByRoomCode = await _pieceMoveService.GetLastestByRoomCode(roomCode);
                    if(getLastestByRoomCode.pieceMove != null)
                    {
                        await Clients.Group(roomCode).SendAsync("ReceiveBoard", getLastestByRoomCode.pieceMove.ChessBoard);
                    }
                }
                else
                {
                    await Clients.Group(roomCode).SendAsync("ReceiveErrorAlert", "Mất kết nối với các người chơi, không thể load game");
                }
            }
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public string ConvertToChessNotation(int startRow, int startCol, int endRow, int endCol, string pieceName)
        {
            char[] columns = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i' };

            string startCoord = $"{columns[startCol]}{(startRow + 1)}";
            string endCoord = $"{columns[endCol]}{(endRow + 1)}";

            char direction;
            if (startRow == endRow)
            {
                direction = '.';
            }
            else if (startCol == endCol)
            {
                direction = '+';
            }
            else
            {
                direction = '=';
            }

            int steps;
            if (direction == '.')
            {
                steps = Math.Abs(endCol - startCol);
            }
            else if (direction == '+')
            {
                steps = Math.Abs(endRow - startRow);
            }
            else
            {
                steps = Math.Abs(endCol - startCol);
            }

            char moveType;
            if (endRow > startRow)
            {
                moveType = '+';
            }
            else if (endRow < startRow)
            {
                moveType = '=';
            }
            else
            {
                moveType = '.'; // Đặt giá trị mặc định nếu không có sự di chuyển theo dòng.
            }

            return $"{pieceName}{startCoord}{direction}{moveType}{steps}";
        }
        //Thêm roomCode và JWT vào hàm sendMovePiece
        public async Task SendMovePiece(int fromRow, int fromCol, int toRow, int toCol, string roomCode, string jwt)
        {
            var room = _dbContext.Rooms.FirstOrDefault(r => r.Code == roomCode);
            Context.GetHttpContext().Items["jwt"] = jwt;
            var infoUser = await _userService.GetUserInfo();
            if (infoUser.statusCode == 1)
            {
                // Kiểm tra Turn có hợp lệ không
                var getLastestByRoomCode = await _pieceMoveService.GetLastestByRoomCode(roomCode);
                if (getLastestByRoomCode.pieceMove != null)
                {
                    if(getLastestByRoomCode.pieceMove.PlayerUserId != infoUser.data.Id)
                    {
                        var boardData = getLastestByRoomCode.pieceMove.ChessBoard;
                        var board = new Board(boardData);
                        var selectPiece = board.GetInfoAtPosition(fromRow, fromCol);
                        var initPiece = board.pieces[selectPiece.Type];
                        List<ChessPosition> listOfPossibleMoves = initPiece.CanPieceMoves(fromRow, fromCol, board);
                        ChessPosition targetPosition = new ChessPosition(toRow, toCol);

                        if (listOfPossibleMoves.Find(item => item.Row == targetPosition.Row && item.Col == targetPosition.Col) != null)
                        {
                            // Kiểm tra vị trí vua, còn hay không
                            // Chiến thắng hay thua xử lí ở đây
                            board.chessPieces[toRow, toCol] = board.chessPieces[fromRow, fromCol];
                            board.chessPieces[fromRow, fromCol] = "0";
                            var updatedBoardData = board.ToBoardData();

                            await _pieceMoveService.Create(new Application.DTOs.PieceMove.CreatePieceMoveDto
                            {
                                MatchId = getLastestByRoomCode.pieceMove.MatchId,
                                PlayerUserId = infoUser.data.Id,
                                MoveContent = ConvertToChessNotation(fromRow, fromCol, toRow, toCol, selectPiece.Type),
                                ChessBoard = updatedBoardData,
                                RoomCode = roomCode,
                                Side = selectPiece.Side,
                            });
                            await Clients.Group(roomCode).SendAsync("ReceiveMovePiece", fromRow, fromCol, toRow, toCol);
                            await Clients.Group(roomCode).SendAsync("ReceiveBoard", updatedBoardData);
                            //await LoadGame(roomCode, infoUser.data);
                            // Trả về danh sách PieceMove
                            // Trong đó sẽ có bàn cờ

                            // Đã tìm thấy vị trí
                        }
                        else
                        {
                            // Không tìm thấy vị trí trong danh sách
                        }
                    }
                    else
                    {
                        // Chưa đến lượt của bạn
                    }
                }
                
            }
                               
            //await Clients.All.SendAsync("ReceiveMovePiece", fromRow, fromCol, toRow, toCol);
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
        public async Task RemoveConnection(string userId, string roomCode)
        {
            if (UserConnection.TryGetValue(userId, out var connectionId) && UserConnection[userId] == connectionId)
            {
                await Groups.RemoveFromGroupAsync(connectionId, roomCode);
                UserConnection.Remove(userId);
            }

        }

        public override async Task OnConnectedAsync()
        {
            var roomCode = Context.GetHttpContext().Request.Query["roomCode"].ToString();
            var jwtToken = Context.GetHttpContext().Request.Query["jwt"].ToString();
            Context.GetHttpContext().Items["jwt"] = jwtToken;

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
                await Clients.Group(roomCode).SendAsync("ReceiveMessage","SYSTEM","" + infoUser.data.UserName + " đã kết nối tới phòng");
            }
            //var isPlaying = Boards.TryGetValue(roomCode, out var board);
            var match = _dbContext.Matches.FirstOrDefault(x => x.RoomCode == roomCode && x.MatchStatus == Domain.Enums.MatchStatus.playing);
            if (match == null)
            {
                //var boardData = "RX,RM,RT,RS,RV,RS,RT,RM,RX,0,0,0,0,0,0,0,0,0,0,RP,0,0,0,0,0,RP,0,RC,0,RC,0,RC,0,RC,0,RC,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,BC,0,BC,0,BC,0,BC,0,BC,0,BP,0,0,0,0,0,BP,0,0,0,0,0,0,0,0,0,0,BX,BM,BT,BS,BV,BS,BT,BM,BX";
                //Boards.Add(roomCode, new Board(boardData));

                //board = Boards[roomCode];

                await CreateGame(roomCode, infoUser.data);
            }
            else
            {
                await LoadGame(roomCode, infoUser.data);
            }
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var roomCode = Context.GetHttpContext().Request.Query["roomCode"].ToString();
            var jwtToken = Context.GetHttpContext().Request.Query["jwt"].ToString();

            Context.GetHttpContext().Items["jwt"] = jwtToken;

            var infoUser = await _userService.GetUserInfo();
            await RemoveConnection(infoUser.data.Id, roomCode);
            await Clients.Group(roomCode).SendAsync("ReceiveMessage", "SYSTEM", "" + infoUser.data.UserName + " đã rời khỏi phòng");
        }
    }
}
