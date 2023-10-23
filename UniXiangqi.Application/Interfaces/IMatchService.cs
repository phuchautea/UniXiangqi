using UniXiangqi.Application.DTOs.Match;
using UniXiangqi.Domain.Entities;
using UniXiangqi.Domain.Enums;

namespace UniXiangqi.Application.Interfaces
{
    public interface IMatchService
    {
        Task<(int statusCode, string message,string RoomId)> Create(CreateMatchRequest request);
        Task<(int statusCode, string message, MatchStatus newStatus)> UpdateMatchStatus(MatchStatusDto request);

        Task<(int statusCode, string message, Match match)> GetById(String matchId);
    }
}
