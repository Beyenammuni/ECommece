using ECommeceSystem.EF.Models;
using ECommerceSystem.Core.Result;
using ECommerceSystem.Domain.DTOs.OrderDtos.Request;
using ECommerceSystem.Domain.DTOs.OrderDtos.Response;
using ECommerceSystem.Domain.DTOs.UserDtos.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceSystem.Domain.IServices
{
    public interface IUserService
    {
        Task<Result<List<UserDto>>> GetAllUsersAsync();
        Task<Result<UserDto>> GetUserByIdAsync(int id);
        Task<Result<UserModel>> CreateUserAsync(CreateUserDto dto);
        Task<Result<UserDto>> UpdateUserAsync(int id, UpdateUser updateUser);
        Task<Result<bool>> DeleteUserAsync(int id);

    }
}
