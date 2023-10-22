using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniXiangqi.Domain.Entities
{
    [Table("PieceMoves")]
    public class PieceMove : BaseEntity
    {
        [Key]
        public string MatchId { get; set; } = String.Empty;
        public string PlayerUserId { get; set; } = String.Empty;
        public string MoveContent { get; set; } = String.Empty;
        public string ChessBoard { get; set; } = String.Empty;
    }
}
