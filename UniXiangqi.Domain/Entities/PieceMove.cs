using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UniXiangqi.Domain.Identity;

namespace UniXiangqi.Domain.Entities
{
    [Table("PieceMoves")]
    public class PieceMove : BaseEntity
    {
        public string? MatchId { get; set; }
        public virtual Match? Match { get; set; }
        public string? PlayerUserId { get; set; }
        public virtual ApplicationUser? PlayerUser { get; set; }
        public string MoveContent { get; set; } = String.Empty;
        public string ChessBoard { get; set; } = String.Empty;
    }
}
