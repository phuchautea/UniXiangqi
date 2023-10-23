using Microsoft.AspNetCore.Mvc;
using UniXiangqi.Application.DTOs.Match;
using UniXiangqi.Application.Interfaces;

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
        [Route("")]
        public async Task<IActionResult> Create([FromBody] CreateMatchRequest request)
        {
            try
            {
                var result = await matchService.Create(request);

                if (result.statusCode == 1)
                {
                    return Ok(new { Message = result.message, MatchId = result.MatchId });
                }
                else
                {
                    return BadRequest(new { Message = result.message, Error = result.MatchId });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Server Error", Error = ex.Message });
            }
        }
        [HttpPost]
        [Route("updateStatus")]
        public async Task<IActionResult> UpdateMatchStatus([FromBody] MatchStatusDto request)
        {
            try
            {
                var result = await matchService.UpdateMatchStatus(request);
                if (result.statusCode == 1)
                {
                    return Ok(new { Message = result.message, Status = result.newStatus  });
                }
                else
                {
                    return BadRequest(new { Message = result.message, Error = result.newStatus });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Server Error", Error = ex.Message });
            }
        }

    }
}
