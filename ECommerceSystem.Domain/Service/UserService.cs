using ECommeceSystem.EF.Models;
using ECommerceSystem.Core.Result;
using ECommerceSystem.Domain.DTOs.OrderDtos.Request;
using ECommerceSystem.Domain.DTOs.OrderDtos.Response;
using ECommerceSystem.Domain.DTOs.UserDtos.Request;
using ECommerceSystem.Domain.IServices;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace ECommerceSystem.Domain.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unit;

        public UserService(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public async Task<Result<UserModel>> CreateUserAsync(CreateUserDto dto)
        {
            if(string.IsNullOrWhiteSpace(dto.FullName))
                return Result<UserModel>.Failure("FullName is required");
            if(string.IsNullOrWhiteSpace(dto.Email))
                return Result<UserModel>.Failure("Email is required");
             if(string.IsNullOrWhiteSpace(dto.Role))
                return Result<UserModel>.Failure("Role is required");
             var user = new UserModel
             {
                 FullName = dto.FullName,
                 Email = dto.Email,
                 Role = dto.Role
             };
            await _unit.Users.AddAsync(user);
            await _unit.Complete();
            return Result<UserModel>.Success(user, "User Created Successfully");
        }

        public async Task<Result<bool>> DeleteUserAsync(int id)
        {
            var user = await _unit.Users.GetByIdAsync(id);
            if (user == null)
                return Result<bool>.NotFound("User not found");
            await _unit.Complete();
            return Result<bool>.Success(true, "User Deleted Successfully");
        }

        public async Task<Result<UserDto>> GetUserByIdAsync(int id)
        {
            var user = await _unit.Users.GetByIdAsync(id);
            if (user == null)
                return Result<UserDto>.NotFound("User not found");
            var dto = new UserDto
            {
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role
            };
            return Result<UserDto>.Success(dto);
        }

        public async Task<Result<List<UserDto>>> GetAllUsersAsync()
        {
            var users = await _unit.Users.GetAllAsync();
            var result = users.Select(u => new UserDto
            {

                FullName = u.FullName,
                Email = u.Email,
                Role = u.Role
            }).ToList();
            return Result<List<UserDto>>.Success(result, "Data Retrieved Successfully");
        }

        public async Task<Result<UserDto>> UpdateUserAsync(int id, UpdateUser updateUser)
        {
            var user = await _unit.Users.GetByIdAsync(id);
            if (user == null)
                return Result<UserDto>.NotFound("User not found");
            if (!string.IsNullOrWhiteSpace(updateUser.FullName))
                user.FullName = updateUser.FullName;
            if (!string.IsNullOrWhiteSpace(updateUser.Email))
                user.Email = updateUser.Email;
            if (!string.IsNullOrWhiteSpace(updateUser.Role))
                user.Role = updateUser.Role;
            _unit.Users.Update(user);
            await _unit.Complete();
            var dto = new UserDto
            {
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role
            };
            return Result<UserDto>.Success(dto, "User Updated Successfully");
        }
    }
}
