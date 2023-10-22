using UniXiangqi.Application.DTOs.Room;
using UniXiangqi.Domain.Entities;

namespace UniXiangqi.Application.Interfaces
{
    public interface IRoomService
    {
        Task<(int statusCode, string message, string roomCode)> Create (CreateRoomRequest request);

    }
}
