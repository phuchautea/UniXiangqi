using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UniXiangqi.Application.DTOs.Match;
using UniXiangqi.Application.Interfaces;
using UniXiangqi.Domain.Entities;
using UniXiangqi.Domain.Enums;
using UniXiangqi.Domain.Identity;
using UniXiangqi.Infrastructure.Persistence;

namespace UniXiangqi.Infrastructure.Services
{
    public class MatchService : IMatchService
    {
        private ApplicationDbContext _dbContext;
        private IUserService _userService;
        public MatchService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IUserService userService)
        {
            this._dbContext = dbContext;
            _userService = userService;
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
                    match.RoomCode = room.Code;
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
        public async Task<(int statusCode, string message, Domain.Entities.Match match)> GetByRoomCode(string roomCode)
        {
            try
            {
                if (string.IsNullOrEmpty(roomCode))
                {
                    return (0, "Mã phòng không hợp lệ", null);
                }
                var room = await _dbContext.Rooms.FirstOrDefaultAsync(c => c.Code == roomCode);
                if (room != null)
                {
                    var match = await _dbContext.Matches.FirstOrDefaultAsync(r => r.RoomId == room.Id);
                    if (match != null)
                    {
                        return (1, $"Lấy trận đấu theo mã phòng: {roomCode} thành công", match);
                    }
                }

                return (0, $"Không tìm thấy trận đấu với mã phòng: {roomCode}", null);
            }
            catch (Exception ex)
            {
                return (0, "Đã có lỗi xảy ra: " + ex.Message, null);
            }
        }

        public async Task<(int statusCode, string message, InfoMatchResponse infoMatchResponse)> GetInfoMatch(string matchId)
        {
            var infoUser = await _userService.GetUserInfo();
            var match = await _dbContext.Matches.FirstOrDefaultAsync(r => r.Id == matchId);
            var pieceMoves = await _dbContext.PieceMoves.Where(p => p.MatchId == matchId).OrderByDescending(p => p.CreatedAt).ToListAsync();
            var infoMatchResponse = new InfoMatchResponse {
                Turn = match.Turn,
                RedUserId = match.RedUserId,
                BlackUserId = match.BlackUserId,
                IsYourTurn = infoUser.data.Id == match.Turn,
                pieceMoves = pieceMoves,
            };
            return (1, "Thành công", infoMatchResponse);
        }
    }
}
