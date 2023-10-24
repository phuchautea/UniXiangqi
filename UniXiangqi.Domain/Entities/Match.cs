using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UniXiangqi.Domain.Enums;

namespace UniXiangqi.Domain.Entities
{
    [Table("Matches")]
    public class Match : BaseEntity
    {
        public string? RoomId { get; set; }
        public string? RoomCode { get; set; }
        public virtual Room? Room { get; set; }
        public string RedUserId { get; set; } = String.Empty;
        public string BlackUserId { get; set; } = String.Empty;
        public string WinnerUserId { get; set; } = String.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Turn { get; set; } = String.Empty;
        public DateTime NextTurn { get; set; } = DateTime.UtcNow;
        public MatchStatus MatchStatus { get; set; }
    }
}
