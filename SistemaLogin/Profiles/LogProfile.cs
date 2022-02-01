using AutoMapper;
using SistemaLogin.Dtos.Log;
using SistemaLogin.Models;

namespace SistemaLogin.Profiles
{
    public class LogProfile : Profile
    {
        public LogProfile()
        {
            CreateMap<CreateLogDto, Log>();
            CreateMap<Log, CreateLogDto>();
        }
    }
}