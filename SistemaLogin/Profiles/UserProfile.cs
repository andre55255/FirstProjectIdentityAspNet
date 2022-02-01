using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SistemaLogin.Dtos.User;
using SistemaLogin.Models;

namespace SistemaLogin.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, CreateUserDto>();
            CreateMap<CreateUserDto, User>();
            CreateMap<User, IdentityUser<int>>();
            CreateMap<IdentityUser<int>, User>();
        }
    }
}