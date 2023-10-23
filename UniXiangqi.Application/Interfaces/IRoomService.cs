using UniXiangqi.Application.DTOs.Room;
using UniXiangqi.Domain.Entities;

namespace UniXiangqi.Application.Interfaces
{
    public interface IRoomService
    {
        Task<bool> HasOpponent(string roomCode);
        Task<(int statusCode, string message, string roomCode)> Create(CreateRoomRequest request);
        Task<(int statusCode, string message, IEnumerable<RoomWithUserNameReponse> rooms)> GetAll();
        Task<(int statusCode, string message, Room room)> GetByCode(string roomCode);
        Task<(int statusCode, string message, IEnumerable<Room> rooms)> GetByUserId(string userId);
    }
}
