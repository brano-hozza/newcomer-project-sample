using System.Collections.Generic;
using WebApi.DTOs;
using WebApi.Helpers;
using WebApi.Models;

namespace WebApi.Services
{
    public interface IUserService
    {
        Result<User> Create(UserDTO dto);
        Result<User> Update(int id, UserDTO dto);
        Result<User> Delete(int id);
        Result<User> Get(int id);
        Result<List<User>> GetAll();
    }
}