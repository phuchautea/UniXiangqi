using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UniXiangqi.Application.DTOs.User;
using UniXiangqi.Application.Interfaces;
using UniXiangqi.Infrastructure.Services;

namespace UniXiangqi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService userService;
        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }
        // POST: api/users/register
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var result = await userService.Register(request);

                if (result.statusCode == 1)
                {
                    return Ok(new { Message = result.message });
                }
                else
                {
                    return Unauthorized(new { Message = result.message });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Server Error", Error = ex.Message });
            }
        }
        // POST: api/users/login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var result = await userService.Login(request);

                if (result.statusCode == 1)
                {
                    return Ok(new { Token = result.message });
                }
                else
                {
                    return Unauthorized(new { Message = result.message });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Server Error", Error = ex.Message });
            }
        }
        // GET: api/users/info
        [HttpGet]
        [Route("info")]
        public async Task<IActionResult> GetUserInfo()
        {
            try
            {
                var result = await userService.GetUserInfo();

                if (result.statusCode == 1)
                {
                    return Ok(new { User = result.data });
                }
                else
                {
                    return Unauthorized(new { Message = result.message });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Server Error", Error = ex.Message });
            }
        }
    }
}
