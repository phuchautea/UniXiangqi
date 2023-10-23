using UniXiangqi.Application.DTOs.Match;
using UniXiangqi.Domain.Enums;

namespace UniXiangqi.Application.Interfaces
{
    public interface IMatchService
    {
        Task<(int statusCode, string message, string MatchId)> Create(CreateMatchRequest request);
        Task<(int statusCode, string message, MatchStatus newStatus)> UpdateMatchStatus(MatchStatusDto request);
    }
}
