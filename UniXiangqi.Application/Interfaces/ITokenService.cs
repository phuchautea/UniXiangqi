using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace UniXiangqi.Application.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(IEnumerable<Claim> claims);
        JwtSecurityToken ReadToken(string jwtToken);
    }
}
