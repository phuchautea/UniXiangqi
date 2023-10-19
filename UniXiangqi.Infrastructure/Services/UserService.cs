using Microsoft.AspNetCore.Identity;
using UniXiangqi.Application.DTOs.User;
using UniXiangqi.Application.Interfaces;
using UniXiangqi.Infrastructure.Identity;

namespace UniXiangqi.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        public UserService(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<(int statusCode, string message)> Register(RegisterRequest request)
        {
            var userExists = await userManager.FindByNameAsync(request.Username);
            if (userExists != null)
                return (0, "Username đã tồn tại.");

            ApplicationUser user = new()
            {
                Email = request.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = request.Username,
            };

            var createUserResult = await userManager.CreateAsync(user, request.Password);
            if (!createUserResult.Succeeded)
                return (0, "Đã có lỗi xảy ra khi tạo tài khoản.");

            return (1, "Tạo tài khoản thành công!");
        }

    }
}
