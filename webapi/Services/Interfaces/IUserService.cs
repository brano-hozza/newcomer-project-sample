using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.DTOs;
using WebApi.Helpers;
using WebApi.Models;

namespace WebApi.Services
{
    public interface IUserService
    {
        Task<Result<UserDTO>> Create(UserDTO dto);
        Task<Result<UserDTO>> Update(int id, UserDTO dto);
        Task<Result<UserDTO>> Delete(int id);
        Task<Result<UserDTO>> DeleteSoft(int id);
        Task<Result<UserDTO>> Get(int id);
        Task<Result<List<UserDTO>>> GetAll();
        Task<Result<List<UserDTO>>> GetAllOld();
        Task<Result<List<PositionChangeDTO>>> GetPositionChanges(int id);

    }
}