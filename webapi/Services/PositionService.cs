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
        async public Task<Result<Position>> Create(PositionDTO dto)
        {
            var position = _mapper.Map<Position>(dto);
            _context.Positions.Add(position);
            await _context.SaveChangesAsync();
            return Result<Position>.Ok(position);
        }
        async public Task<Result<Position>> Delete(int id)
        {
            var position = _context.Positions.Find(id);
            if (position == null)
            {
                return Result<Position>.Fail(position!, ErrorType.PositionNotFound, new System.Exception("Position not found"));
            }
            try // Handle reference error
            {
                _context.Positions.Remove(position);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Result<Position>.Fail(position!, ErrorType.PositionReferenceExists, new System.Exception("Reference error"));
            }
            return Result<Position>.Ok(position);
        }
        async public Task<Result<List<Position>>> GetAll()
        {
            var positions = await _context.Positions.ToListAsync();
            return Result<List<Position>>.Ok(positions);
        }
    }
}