using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UniXiangqi.Domain.Entities;
using UniXiangqi.Domain.Identity;

namespace UniXiangqi.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<UserInRoom> UserInRooms { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Room>()
                .HasOne(x => x.HostUser)
                .WithMany()
                .HasForeignKey(x => x.HostUserId);
            //.OnDelete(DeleteBehavior.Restrict); // Đặt tùy chọn ON DELETE NO ACTION

            modelBuilder.Entity<Room>()
                .HasOne(x => x.OpponentUser)
                .WithMany()
                .HasForeignKey(x => x.OpponentUserId);
                //.OnDelete(DeleteBehavior.Restrict); // Đặt tùy chọn ON DELETE NO ACTION
        }
    }
}
