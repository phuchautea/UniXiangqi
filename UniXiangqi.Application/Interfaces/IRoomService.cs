using UniXiangqi.Application.DTOs.Room;

namespace UniXiangqi.Application.Interfaces
{
    public interface IRoomService
    {
        Task<bool> HasOpponent(string roomCode);
        Task<(int statusCode, string message, string roomCode)> Create(CreateRoomRequest request);

    }
}
