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
    public class PositionService : IPositionService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public PositionService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        async public Task<Result<PositionDTO>> Create(PositionDTO dto)
        {
            var position = _mapper.Map<Position>(dto);
            _context.Positions.Add(position);
            await _context.SaveChangesAsync();
            return Result<PositionDTO>.Ok(_mapper.Map<PositionDTO>(position));
        }
        async public Task<Result<PositionDTO>> Delete(int id)
        {
            var position = _context.Positions.Find(id);
            if (position == null)
            {
                return Result<PositionDTO>.Fail(null!, ErrorType.PositionNotFound, new System.Exception("Position not found"));
            }
            try // Handle reference error
            {
                _context.Positions.Remove(position);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Result<PositionDTO>.Fail(null!, ErrorType.PositionReferenceExists, new System.Exception("Reference error"));
            }
            return Result<PositionDTO>.Ok(_mapper.Map<PositionDTO>(position));
        }
        async public Task<Result<List<PositionDTO>>> GetAll()
        {
            var positions = await _context.Positions.ToListAsync();
            return Result<List<PositionDTO>>.Ok(_mapper.Map<List<PositionDTO>>(positions));
        }

    }
}