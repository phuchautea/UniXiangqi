using UniXiangqi.Application.DTOs.Match;

namespace UniXiangqi.Application.Interfaces
{
    public interface IMatchService
    {
        Task<(int statusCode, string message,string RoomId)> Create(CreateMatchRequest request);
    }
}
