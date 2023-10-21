using Microsoft.AspNetCore.Mvc;
using UniXiangqi.Application.DTOs.Room;
using UniXiangqi.Application.Interfaces;

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
        [Route("/")]
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
    }
}
