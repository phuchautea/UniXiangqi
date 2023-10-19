using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UniXiangqi.Application.DTOs.User;
using UniXiangqi.Application.Interfaces;
using UniXiangqi.Infrastructure.Identity;

namespace UniXiangqi.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private ITokenService tokenService;
        public UserService(UserManager<ApplicationUser> userManager, ITokenService tokenService)
        {
            this.userManager = userManager;
            this.tokenService = tokenService;
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

        public async Task<(int statusCode, string message)> Login(LoginRequest request)
        {
            var user = await userManager.FindByNameAsync(request.Username);
            if (user == null)
                return (0, "Tài khoản/Mật khẩu không hợp lệ");
            if (!await userManager.CheckPasswordAsync(user, request.Password))
                return (0, "Tài khoản/Mật khẩu không hợp lệ");

            var userRoles = await userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
               new Claim(ClaimTypes.Name, user.UserName),
               new Claim(ClaimTypes.NameIdentifier, user.Id),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            string token = tokenService.CreateToken(authClaims);
            return (1, token);
        }

    }
}
