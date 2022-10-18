using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.DTOs;
using WebApi.Helpers;
using WebApi.Models;

namespace WebApi.Services
{
    public interface IPositionService
    {
        Task<Result<Position>> Create(PositionDTO dto);
        Task<Result<Position>> Delete(int id);
        Task<Result<List<Position>>> GetAll();
    }
}