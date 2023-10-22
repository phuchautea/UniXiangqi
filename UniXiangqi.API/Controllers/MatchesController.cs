using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniXiangqi.Application.DTOs.Match;
using UniXiangqi.Application.Interfaces;
using UniXiangqi.Infrastructure.Services;

namespace UniXiangqi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private IMatchService matchService;
        public MatchesController(IMatchService matchService)
        {
            this.matchService = matchService;
        }
        [HttpPost]
        [Route("/matches")]
        public async Task<IActionResult> Create([FromBody] CreateMatchRequest request)
        {
            try
            {
                var result = await matchService.Create(request);

                if (result.statusCode == 1)
                {
                    return Ok(new { Message = result.message, RoomId = result.RoomId });
                }
                else
                {
                    return BadRequest(new { Message = result.message, Error = result.RoomId });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Server Error", Error = ex.Message });
            }
        }

    }
}
