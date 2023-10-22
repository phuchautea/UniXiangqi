using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniXiangqi.Application.DTOs.User;
using UniXiangqi.Domain.Identity;

namespace UniXiangqi.Application.Interfaces
{
    public interface IUserService
    {
        Task<(int statusCode, string message)> Register(RegisterRequest request);
        Task<(int statusCode, string message)> Login(LoginRequest request);
        Task<(int statusCode, string message, InfoResponse data)> GetUserInfo();
        string GetJWT();
    }
}
