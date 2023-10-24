using Microsoft.AspNetCore.Mvc;
using UniXiangqi.Application.DTOs.PieceMove;
using UniXiangqi.Application.Interfaces;
using UniXiangqi.Domain.Entities;

namespace UniXiangqi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PieceMovesController : ControllerBase
    {
        private IPieceMoveService pieceMoveService;
        public PieceMovesController(IPieceMoveService pieceMoveService)
        {
            this.pieceMoveService = pieceMoveService;
        }
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create([FromBody] CreatePieceMoveDto pieceMoveDto)
        {
            try
            {
                var result = await pieceMoveService.Create(pieceMoveDto);

                if (result.statusCode == 1)
                {
                    return Ok(new { Message = result.message, RoomId = result.PieceMoveId });
                }
                else
                {
                    return BadRequest(new { Message = result.message, Error = result.PieceMoveId });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Server Error", Error = ex.Message });
            }
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await pieceMoveService.GetAll();
                if (result.statusCode == 1)
                {
                    return Ok(new { Message = result.message, rooms = result.pieceMoves });
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
        [Route("{matchId}")]
        public async Task<IActionResult> GetByCode(string matchId)
        {
            try
            {
                var result = await pieceMoveService.GetByMatchId(matchId);
                if (result.statusCode == 1)
                {
                    return Ok(new { Message = result.message, room = result.pieceMoves });
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
        [Route("GetLastestByRoomCode/{roomCode}")]
        public async Task<IActionResult> GetLastestByRoomCode(string roomCode)
        {
            try
            {
                var result = await pieceMoveService.GetLastestByRoomCode(roomCode);
                if (result.statusCode == 1)
                {
                    return Ok(new { Message = result.message, pieceMove = result.pieceMove });
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
