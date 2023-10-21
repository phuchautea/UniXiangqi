using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UniXiangqi.Domain.Entities;
using UniXiangqi.Infrastructure.Identity;

namespace UniXiangqi.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<UserInRoom> UserInRooms { get; set; }
    }
}
