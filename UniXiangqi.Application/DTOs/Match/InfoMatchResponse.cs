using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniXiangqi.Application.DTOs.Match
{
    public class InfoMatchResponse
    {
        public string Turn { get; set; } = string.Empty;
        public bool IsYourTurn { get; set; } = false;
        public string RedUserId { get; set; } = string.Empty;
        public string BlackUserId { get; set; } = string.Empty;
        public List<Domain.Entities.PieceMove> pieceMoves { get; set; }
    }
}
