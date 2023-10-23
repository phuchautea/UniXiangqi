using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniXiangqi.Domain.Enums;

namespace UniXiangqi.Application.DTOs.Match
{
    public class MatchStatusDto
    {
        public string matchId { get; set; }
        public MatchStatus newStatus { get; set; }

    }
}
