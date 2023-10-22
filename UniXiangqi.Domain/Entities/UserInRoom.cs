using System.ComponentModel.DataAnnotations.Schema;
using UniXiangqi.Domain.Identity;

namespace UniXiangqi.Domain.Entities
{
    [Table("UserInRooms")]
    public class UserInRoom : BaseEntity
    {
        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }
        public string? RoomId { get; set; }
        public virtual Room? Room { get; set; }
        public bool IsPlayer { get; set; } = false;
    }
}
