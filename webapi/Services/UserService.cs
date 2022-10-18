using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.DTOs;
using WebApi.Helpers;
using WebApi.Models;

namespace WebApi.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UserService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        async public Task<bool> UserExists(UserDTO dto)
        {
            var _u = await _context.Users.Select(u => u).Where(u => u.Name == dto.Name && u.Surname == dto.Surname).FirstOrDefaultAsync();
            return _u != null;
        }
        async public Task<Result<UserDTO>> Create(UserDTO dto)
        {
            if (await UserExists(dto))
            {
                return Result<UserDTO>.Fail(null!, ErrorType.UserExists, new System.Exception("User with this name already exists"));
            }
            var position = await _context.Positions.FindAsync(dto.Position);
            // Check position existence
            if (position == null)
            {
                return Result<UserDTO>.Fail(null!, ErrorType.PositionNotFound, new System.Exception("Position not found"));
            }

            var user = new User(position, dto);
            _context.Users.Add(user);
            // Set position change
            _context.PositionChanges.Add(new() { Position = position, User = user, StartDate = dto.StartDate });
            dto.Id = user.Id;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Result<UserDTO>.Fail(null!, ErrorType.DbError, new System.Exception("Db error"));
            }
            return Result<UserDTO>.Ok(_mapper.Map<UserDTO>(user));
        }
        async public Task<Result<UserDTO>> Update(int id, UserDTO dto)
        {
            var user = await _context.Users.Include(u => u.Position).Where(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null)
            {
                return Result<UserDTO>.Fail(null!, ErrorType.UserNotFound, new System.Exception("User not found"));
            }
            var position = await _context.Positions.FindAsync(dto.Position);
            if (position == null)
            {
                return Result<UserDTO>.Fail(null!, ErrorType.PositionNotFound, new System.Exception("Position not found"));
            }

            // Check if position changed
            if (user.Position.Id != dto.Position)
            {
                // Get last position
                var change = await _context.PositionChanges.Where(pc => pc.User.Id == id).OrderByDescending(pc => pc.StartDate).FirstOrDefaultAsync();
                if (change == null)
                {
                    return Result<UserDTO>.Fail(null!, ErrorType.PositionChangeNotFound, new System.Exception("Position change not found"));
                }
                // Set end date
                change.EndDate = DateTime.Now;
                _context.Entry(change).State = EntityState.Modified;

                // Add new position change
                _context.PositionChanges.Add(new()
                {
                    StartDate = DateTime.Now,
                    User = user,
                    Position = position
                });
            }
            // Update user info
            _context.Entry(user).State = EntityState.Modified;
            user.Position = position;
            user.Address = dto.Address;
            user.Salary = dto.Salary;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Result<UserDTO>.Fail(null!, ErrorType.DbError, new System.Exception("Database error"));
            }
            return Result<UserDTO>.Ok(_mapper.Map<UserDTO>(user));
        }
        public Task<Result<UserDTO>> DeleteSoft(int id)
        {
            return this.DeleteUser(id, true);
        }
        public Task<Result<UserDTO>> Delete(int id)
        {
            return this.DeleteUser(id);
        }
        async public Task<Result<UserDTO>> Get(int id)
        {
            var user = await _context.Users.Include(u => u.Position).Where(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null)
            {
                return Result<UserDTO>.Fail(null!, ErrorType.UserNotFound, new System.Exception("User not found"));
            }
            return Result<UserDTO>.Ok(_mapper.Map<UserDTO>(user));
        }
        async public Task<Result<List<UserDTO>>> GetAll()
        {
            var users = await _context.Users.Include(user => user.Position).Where(user => user.ResignedDate == null).ToListAsync();
            return Result<List<UserDTO>>.Ok(_mapper.Map<List<UserDTO>>(users));
        }
        async public Task<Result<List<UserDTO>>> GetAllOld()
        {
            var users = await _context.Users.Include(user => user.Position).Where(user => user.ResignedDate != null).ToListAsync();
            return Result<List<UserDTO>>.Ok(_mapper.Map<List<UserDTO>>(users));
        }

        /**
       * Method to delete user
       * Set soft=true for soft delete
       */
        private async Task<Result<UserDTO>> DeleteUser(int id, bool soft = false)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return Result<UserDTO>.Fail(null!, ErrorType.UserNotFound, new System.Exception("User not found"));
            }
            if (soft)
            {

                var change = await _context.PositionChanges.Where(pc => pc.User.Id == id).OrderByDescending(pc => pc.StartDate).FirstOrDefaultAsync();
                if (change == null)
                {
                    return Result<UserDTO>.Fail(null!, ErrorType.PositionChangeNotFound, new System.Exception("Position change not found"));
                }
                change.EndDate = DateTime.Now;
                user.ResignedDate = DateTime.Now;
                _context.Entry(user).State = EntityState.Modified;
            }
            else
            {
                _context.Users.Remove(user);
            }
            await _context.SaveChangesAsync();

            return Result<UserDTO>.Ok(_mapper.Map<UserDTO>(user));
        }

        async public Task<Result<List<PositionChangeDTO>>> GetPositionChanges(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return Result<List<PositionChangeDTO>>.Fail(null!, ErrorType.UserNotFound, new System.Exception("User not found"));
            }
            var ch = await _context.PositionChanges.Include(pc => pc.Position).Where(pc => pc.User.Id == id).ToListAsync();
            return Result<List<PositionChangeDTO>>.Ok(_mapper.Map<List<PositionChangeDTO>>(ch));
        }
    }
}