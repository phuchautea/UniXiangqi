using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UniXiangqi.Application.DTOs.Room;
using UniXiangqi.Application.Interfaces;
using UniXiangqi.Domain.Entities;
using UniXiangqi.Domain.Identity;
using UniXiangqi.Infrastructure.Persistence;

namespace UniXiangqi.Infrastructure.Services
{
    public class RoomService : IRoomService
    {
        private ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        public RoomService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager) { 
            this._dbContext = dbContext;
            this.userManager = userManager;
        }

        public async Task<bool> HasOpponent(string roomCode)
        {
            return await _dbContext.Rooms.AnyAsync(room => room.Code == roomCode && room.OpponentUserId != null);
        }

        public async Task<(int statusCode, string message, string roomCode)> Create(CreateRoomRequest request)
        {
            var hostUser = await userManager.FindByIdAsync(request.HostUserId);
            try
            {
                bool isRedTurn = request.HostSide == "Random" ? new Random().Next(2) == 0 : request.HostSide == "Red";
                bool isHostTurn = request.HostSide == "Random" ? isRedTurn : request.HostSide == "Red";
                var room = new Room()
                {
                    GameTimer = request.GameTimer,
                    MoveTimer = request.MoveTimer,
                    HostUserId = hostUser.Id,
                    HostSide = request.HostSide,
                    Password = request.Password,
                    IsRated = request.IsRated,
                    IsRedTurn = isRedTurn,
                    IsHostTurn = isHostTurn,
                    TotalUser = 1,
                };

                var createRoomResult = await _dbContext.Rooms.AddAsync(room);
                await _dbContext.SaveChangesAsync();

                return (1, "Tạo phòng thành công", room.Code);
            }
            catch(Exception ex)
            {
                return (0, "Đã có lỗi xảy ra", ex.Message.ToString());
            }
            
        }
        //Get all Rooms (with hostUserName and opponentUserName)
        public async Task<(int statusCode, string message, IEnumerable<GetAllRoomsResponse> rooms)> GetAll()
        {
            try { 
                var rooms = await _dbContext.Rooms.ToListAsync();
                // Tạo danh sách RoomWithUserNameReponse để chứa thông tin phòng với userName và IsRated
                var roomsWithUserNames = new List<GetAllRoomsResponse>();
                foreach (var room in rooms)
                {
                    var hostUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == room.HostUserId);
                    var hostUserName = hostUser != null ? hostUser.UserName : null;
                    var opponentUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == room.OpponentUserId);
                    var opponentUserName = opponentUser != null ? opponentUser.UserName : null;
                    var roomWithUserNameReponse = new GetAllRoomsResponse
                    {
                        Code = room.Code,
                        HostUserName = hostUserName,
                        OpponentUserName = opponentUserName,
                        IsRated = room.IsRated,
                        GameTimer = room.GameTimer,
                        MoveTimer = room.MoveTimer
                    };
                    roomsWithUserNames.Add(roomWithUserNameReponse);
                }
            return (1, "Lấy danh sách thành công", roomsWithUserNames);
            }
            catch (Exception ex)
            {
                return (0, "Đã có lỗi xảy ra" + ex.Message, null);
            }
        }
        //Get by roomCode
        public async Task<(int statusCode, string message, Room room)> GetByCode(string roomCode)
        {
            try
            {
                var room = await _dbContext.Rooms.FirstOrDefaultAsync(r => r.Code == roomCode);
                return (1, $"Lấy phòng theo code: {roomCode} thành công", room);
            }
            catch (Exception ex)
            {
                return (0, "Đã có lỗi xảy ra" + ex.Message, null);
            }
        }
        //Get by UserId
        public async Task<(int statusCode, string message, IEnumerable<Room> rooms)> GetByUserId(string userId)
        {
            try
            {
                var rooms = await _dbContext.Rooms.Where(r => r.HostUserId == userId || r.OpponentUserId == userId).ToListAsync();
                return (1, $"Lấy danh sách phòng theo userId: {userId} thành công", rooms);
            }
            catch (Exception ex)
            {
                return (0, "Đã có lỗi xảy ra" + ex.Message, null);
            }
        }

    }
}
