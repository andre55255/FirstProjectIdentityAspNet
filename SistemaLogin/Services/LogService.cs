using AutoMapper;
using FluentResults;
using SistemaLogin.Data;
using SistemaLogin.Dtos.Log;
using SistemaLogin.Models;
using System;
using System.Threading.Tasks;

namespace SistemaLogin.Services
{
    public class LogService
    {
        private readonly IMapper _mapper;
        private readonly UserDbContext _dbContext;

        public LogService(IMapper mapper, UserDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<Result> RegisterLog(CreateLogDto logDto)
        {
            try
            {
                Log log = _mapper.Map<Log>(logDto);

                _dbContext.Logs.Add(log);
                await _dbContext.SaveChangesAsync();

                return Result.Ok();
            }
            catch (Exception)
            {
                return Result.Fail("Failed to create log");
            }
        } 
    }
}