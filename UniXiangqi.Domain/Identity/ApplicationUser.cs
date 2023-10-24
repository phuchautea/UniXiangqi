using Microsoft.AspNetCore.Identity;

namespace UniXiangqi.Domain.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public int TotalPoint { get; set; }
    }
}
