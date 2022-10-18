using System.Collections.Generic;
using System.Linq;
using AutoMapper;
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
        public Result<User> Create(UserDTO dto)
        {
            var user = _mapper.Map<User>(dto);
            _context.Users.Add(user);
            _context.SaveChanges();
            return Result<User>.Ok(user);
        }
        public Result<User> Update(int id, UserDTO dto)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return Result<User>.Fail(user!, ErrorType.UserNotFound, new System.Exception("User not found"));
            }
            _mapper.Map(dto, user);
            _context.SaveChanges();
            return Result<User>.Ok(user);
        }
        public Result<User> Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return Result<User>.Fail(user!, ErrorType.UserNotFound, new System.Exception("User not found"));
            }
            _context.Users.Remove(user);
            _context.SaveChanges();
            return Result<User>.Ok(user);
        }
        public Result<User> Get(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return Result<User>.Fail(user!, ErrorType.UserNotFound, new System.Exception("User not found"));
            }
            return Result<User>.Ok(user);
        }
        public Result<List<User>> GetAll()
        {
            var users = _context.Users.ToList();
            return Result<List<User>>.Ok(users);
        }
    }
}