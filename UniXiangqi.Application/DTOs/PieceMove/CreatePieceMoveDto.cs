namespace UniXiangqi.Application.DTOs.PieceMove
{
    public class CreatePieceMoveDto
    {
        public string? MatchId { get; set; }
        public string? PlayerUserId { get; set; }
        public string MoveContent { get; set; } = String.Empty;
        public string ChessBoard { get; set; } = String.Empty;
    }
}
