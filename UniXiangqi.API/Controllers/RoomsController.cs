using Microsoft.AspNetCore.Mvc;
using UniXiangqi.Application.DTOs.Room;
using UniXiangqi.Application.Interfaces;
using UniXiangqi.Infrastructure.Services;

namespace UniXiangqi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private IRoomService roomService;
        public RoomsController(IRoomService roomService)
        {
            this.roomService = roomService;
        }
        // POST: api/rooms
        [HttpPost]
        [Route("/rooms")]
        public async Task<IActionResult> Create([FromBody] CreateRoomRequest request)
        {
            try
            {
                var result = await roomService.Create(request);

                if (result.statusCode == 1)
                {
                    return Ok(new { Message = result.message, RoomCode = result.roomCode });
                }
                else
                {
                    return Unauthorized(new { Message = result.message, Error = result.roomCode });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Server Error", Error = ex.Message });
            }
        }
        [HttpGet]
        [Route("/rooms")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await roomService.GetAll();
                if (result.statusCode == 1)
                {
                    return Ok(new { Message = result.message, rooms = result.rooms });
                }
                else
                {
                    return BadRequest(new { Message = result.message });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Server Error", Error = ex.Message });
            }
        }
        [HttpGet]
        [Route("/room/{roomCode}")]
        public async Task<IActionResult> GetByCode(string roomCode)
        {
            try
            {
                var result = await roomService.GetByCode(roomCode);
                if (result.statusCode == 1)
                {
                    return Ok(new { Message = result.message, room = result.room });
                }
                else
                {
                    return BadRequest(new { Message = result.message });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Server Error", Error = ex.Message });
            }
        }
        [HttpGet]
        [Route("/room/user/{userId}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            try
            {
                var result = await roomService.GetByUserId(userId);
                if (result.statusCode == 1)
                {
                    return Ok(new { Message = result.message, room = result.rooms });
                }
                else
                {
                    return BadRequest(new { Message = result.message });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Server Error", Error = ex.Message });
            }
        }
    }
}
