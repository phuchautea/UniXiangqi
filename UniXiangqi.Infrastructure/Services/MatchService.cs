using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UniXiangqi.Application.DTOs.Match;
using UniXiangqi.Application.Interfaces;
using UniXiangqi.Domain.Enums;
using UniXiangqi.Domain.Identity;
using UniXiangqi.Infrastructure.Persistence;

namespace UniXiangqi.Infrastructure.Services
{
    public class MatchService : IMatchService
    {
        private ApplicationDbContext _dbContext;
        public MatchService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            this._dbContext = dbContext;

        }
        public async Task<(int statusCode, string message,string MatchId)> Create(CreateMatchRequest request)
        {
            try {
                //Lấy các giá trị từ Room tương ứng
                var room = await _dbContext.Rooms.FirstOrDefaultAsync(r => r.Code == request.roomCode);
                var match = new Domain.Entities.Match();
                if (room != null)
                {     
                    

                    match.MatchStatus = Domain.Enums.MatchStatus.playing;
                    match.RoomId = room.Id;
                    match.RedUserId = room.IsHostTurn ? room.HostUserId : room.OpponentUserId;
                    match.BlackUserId = room.IsHostTurn ? room.OpponentUserId : room.HostUserId;
                    match.Turn = room.IsHostTurn ? room.HostUserId : room.OpponentUserId;
                    match.NextTurn = DateTime.Now.AddMinutes(room.MoveTimer);
                    match.StartTime = DateTime.Now;
                    match.EndTime = DateTime.Now.AddMinutes(room.GameTimer);
                    match.WinnerUserId = String.Empty;

                    var createMatch = await _dbContext.Matches.AddAsync(match);
                    await _dbContext.SaveChangesAsync();
                    
                }
                return (1, "Tạo trận đấu thành công", match.Id);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (ví dụ: log lỗi)
                return (0,"Đã có lỗi xảy ra", ex.Message.ToString());
            }
        }
        public async Task<(int statusCode, string message, MatchStatus newStatus)> UpdateMatchStatus(MatchStatusDto request)
        {
            try 
            {
                var match = await _dbContext.Matches.FirstOrDefaultAsync(m => m.Id == request.matchId);
                if (match != null)
                {
                    match.MatchStatus = request.newStatus;
                    await _dbContext.SaveChangesAsync();
                    return (1, "Cập nhật trạng thái cho phòng thành công", request.newStatus);
                }
                else
                {
                    return (0, "Không tìm thấy trận đấu với ID đã cung cấp", request.newStatus);
                }
            }
            catch (Exception ex)
            {
                return (0, "Đã có lỗi xảy ra: " + ex.Message, MatchStatus.pending);
            }
        }
        public async Task<(int statusCode, string message, Domain.Entities.Match match)> GetById(string matchId)
        {
            try
            {
                if (string.IsNullOrEmpty(matchId))
                {
                    return (0, "Mã trận đấu không hợp lệ", null);
                }

                var match = await _dbContext.Matches.FirstOrDefaultAsync(r => r.Id == matchId);

                if (match != null)
                {
                    return (1, $"Lấy trận đấu theo id: {matchId} thành công", match);
                }
                else
                {
                    return (0, $"Không tìm thấy trận đấu với id: {matchId}", null);
                }
            }
            catch (Exception ex)
            {
                return (0, "Đã có lỗi xảy ra: " + ex.Message, null);
            }
        }
    }
}
