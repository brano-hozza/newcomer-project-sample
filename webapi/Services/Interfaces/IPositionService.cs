using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.DTOs;
using WebApi.Helpers;
using WebApi.Models;

namespace WebApi.Services
{
    public interface IPositionService
    {
        Task<Result<PositionDTO>> Create(PositionDTO dto);
        Task<Result<PositionDTO>> Delete(int id);
        Task<Result<List<PositionDTO>>> GetAll();
    }
}