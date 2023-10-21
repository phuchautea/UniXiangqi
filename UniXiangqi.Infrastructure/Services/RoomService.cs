using Microsoft.AspNetCore.Identity;
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
    }
}
