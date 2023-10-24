using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using UniXiangqi.Application.DTOs.User;
using UniXiangqi.Application.Interfaces;
using UniXiangqi.Domain.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UniXiangqi.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private ITokenService tokenService;
        private IHttpContextAccessor httpContextAccessor;
        public UserService(UserManager<ApplicationUser> userManager, ITokenService tokenService, IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.httpContextAccessor = httpContextAccessor;
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
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            string token = tokenService.CreateToken(authClaims);
            return (1, token);
        }
        public async Task<(int statusCode, string message, InfoResponse data)> GetUserInfo()
        {
            var userInfo = new InfoResponse();
            string jwtToken = string.Empty;
            // Lấy từ Context.Items
            if (httpContextAccessor.HttpContext.Items.TryGetValue("jwt", out var jwtFromContext))
            {
                jwtToken = jwtFromContext.ToString();
            }
            else
            {
                // Lấy từ Authorization header
                var authorizationHeader = httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
                if (!string.IsNullOrEmpty(authorizationHeader))
                {
                    jwtToken = authorizationHeader.Trim();
                }
            }

            if (!string.IsNullOrEmpty(jwtToken))
            {
                JwtSecurityToken jwt = tokenService.ReadToken(jwtToken);

                if (jwt != null)
                {
                    string userId = jwt.Claims.First(claim => claim.Type == "nameid").Value;
                    var user = await userManager.FindByIdAsync(userId);
                    if (user == null)
                        return (0, "User không hợp lệ", userInfo);
                    var info = new InfoResponse
                    {
                        Email = user.Email,
                        UserName = user.UserName,
                        Id = user.Id.ToString(),
                    };
                    return (1, "Thành công", info);
                }
                return (0, "Token không hợp lệ", userInfo);
            }
            return (0, "Chưa đăng nhập", userInfo);
            
        }
        public string GetJWT()
        {
            var authorizationHeader = httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                string jwtToken = authorizationHeader.Trim();

                JwtSecurityToken jwt = tokenService.ReadToken(jwtToken);

                if (jwt != null)
                {
                    string userId = jwt.Claims.First(claim => claim.Type == "unique_name").Value;
                    string userName = jwt.Claims.First(claim => claim.Type == "nameid").Value;
                }
                return authorizationHeader.Trim();
            }
            return string.Empty;
        }

        private void UpdateUserRating(string userId, int scoreChange)
        {
            var user = userManager.FindByIdAsync(userId).Result;

            if (user != null)
            {
                user.TotalPoint += scoreChange;
                var updateResult = userManager.UpdateAsync(user).Result;

                if (updateResult.Succeeded)
                {
                    Console.WriteLine($"Điểm xếp hạng của người chơi (ID: {userId}) đã được cập nhật. Điểm thay đổi: {scoreChange}");
                }
                else
                {
                    Console.WriteLine($"Lỗi khi cập nhật điểm xếp hạng cho người chơi (ID: {userId}).");
                    // Xử lý lỗi khi cập nhật
                }
            }
        }
    }
}
