using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UniXiangqi.Application.DTOs.PieceMove;
using UniXiangqi.Application.Interfaces;
using UniXiangqi.Domain.Entities;
using UniXiangqi.Domain.Identity;
using UniXiangqi.Infrastructure.Persistence;

namespace UniXiangqi.Infrastructure.Services
{
    public class PieceMoveService : IPieceMoveService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public PieceMoveService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<(int statusCode, string message, string PieceMoveId)> Create(CreatePieceMoveDto pieceMoveDto)
        {
            try
            {
                // Tạo một đối tượng PieceMove từ DTO
                var pieceMove = new PieceMove
                {
                    MatchId = pieceMoveDto.MatchId,
                    PlayerUserId = pieceMoveDto.PlayerUserId,
                    MoveContent = pieceMoveDto.MoveContent,
                    ChessBoard = pieceMoveDto.ChessBoard
                };

                // Thêm đối tượng PieceMove vào cơ sở dữ liệu
                var createpieceMove = await _dbContext.PieceMoves.AddAsync(pieceMove);
                await _dbContext.SaveChangesAsync();

               

                // Trả về một tuple chứa các thông tin
                return (1, "Tạo bước di chuyển thành công", pieceMove.Id);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (ví dụ: log lỗi)
                return (0, "Đã có lỗi xảy ra", ex.Message);
            }
        }
        //Get by MatchId
        public async Task<(int statusCode, string message, IEnumerable<PieceMove> pieceMoves)> GetByMatchId(string matchId)
        {
            try
            {
                var pieceMoves = await _dbContext.PieceMoves.Where(r => r.MatchId == matchId).ToListAsync();
                return (1, $"Lấy danh sách bước di chuyển theo MatchId: {matchId} thành công", pieceMoves);
            }
            catch (Exception ex)
            {
                return (0, "Đã có lỗi xảy ra" + ex.Message, null);
            }
        }

        

        //Get all piecemoves
        public async Task<(int statusCode, string message, IEnumerable<PieceMove> pieceMoves)> GetAll()
        {
            try
            {
                var pieceMoves = await _dbContext.PieceMoves.ToListAsync();
                return (1, "Lấy danh sách bước di chuyển thành công", pieceMoves);
            }
            catch (Exception ex)
            {
                return (0, "Đã có lỗi xảy ra" + ex.Message, null);
            }
        }
    }
}
