using UniXiangqi.Application.DTOs.PieceMove;
using UniXiangqi.Domain.Entities;

namespace UniXiangqi.Application.Interfaces
{
    public interface IPieceMoveService
    {
        Task<(int statusCode, string message, string PieceMoveId)> Create(CreatePieceMoveDto pieceMoveDto);
        Task<(int statusCode, string message, IEnumerable<PieceMove> pieceMoves)> GetAll();

        Task<(int statusCode, string message, IEnumerable<PieceMove> pieceMoves)> GetByMatchId(string matchId);
        Task<(int statusCode, string message, PieceMove pieceMove)> GetLastestByRoomCode(string roomCode);


    }
}
