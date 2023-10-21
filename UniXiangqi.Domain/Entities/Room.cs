using System.ComponentModel.DataAnnotations.Schema;
using UniXiangqi.Domain.Utilities;
using UniXiangqi.Domain.Identity;
using System.ComponentModel.DataAnnotations;

namespace UniXiangqi.Domain.Entities
{
    [Table("Rooms")]
    public class Room : BaseEntity
    {
        public string Code { get; set; } = RandomUtility.RandomString(7);
        public int GameTimer { get; set; } = 10;
        public int MoveTimer { get; set; } = 2;
        public string? HostUserId { get; set; }
        public virtual ApplicationUser? HostUser { get; set; }
        public string? OpponentUserId { get; set; }
        public virtual ApplicationUser? OpponentUser { get; set; }
        public string HostSide { get; set; } = string.Empty;
        public bool IsRated { get; set; }
        public string Password { get; set; } = string.Empty;
        public int TotalUser { get; set; } = 0;
        public bool IsRedTurn { get; set; }
        public bool IsHostTurn { get; set; }
        public virtual ICollection<Match> Matches { get; set; } = new HashSet<Match>();
        public virtual ICollection<UserInRoom> UserInRooms { get; set; } = new HashSet<UserInRoom>();
    }
}
