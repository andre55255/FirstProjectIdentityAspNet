using AutoMapper;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SistemaLogin.Dtos.Log;
using SistemaLogin.Dtos.User;
using SistemaLogin.Helpers;
using SistemaLogin.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SistemaLogin.Services
{
    public class RegisterService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser<int>> _userManager;
        private readonly EmailService _emailService;
        private readonly LogService _logService;

        public RegisterService(IMapper mapper,
            UserManager<IdentityUser<int>> userManager,
            EmailService emailService, LogService logService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _emailService = emailService;
            _logService = logService;
        }

        public async Task<Result> CreateUser(CreateUserDto userDto)
        {
            try
            {
                User user = _mapper.Map<User>(userDto);

                IdentityUser<int> userIdentity = _mapper.Map<IdentityUser<int>>(user);

                Task<IdentityResult> resultIdentity =
                    _userManager.CreateAsync(userIdentity, userDto.Password);

                if (resultIdentity.Result.Succeeded)
                {
                    string code = await _userManager.GenerateEmailConfirmationTokenAsync(userIdentity);

                    string encodedCode = HttpUtility.UrlEncode(code);

                    await _emailService.SenderEmail(new[] { userIdentity.Email },
                        "Link para ativação de conta", userIdentity.Id,
                        encodedCode);

                    return Result.Ok();
                }

                return Result.Fail("Operation create user failed");
            }
            catch (Exception ex)
            {
                CreateLogDto log = new CreateLogDto
                {
                    Description = ConstMessages.ErroCriarUsuario,
                    Exception = ex.Message,
                    Place = "RegisterService"
                };

                await _logService.RegisterLog(log);

                return Result.Fail("Operation create user failed");
            }
        }

        public async Task<Result> ActiveUser(ActiveUserDto userDto)
        {
            try
            {
                IdentityUser<int> identityUser = await
                    _userManager.Users.FirstOrDefaultAsync(x => x.Id == userDto.UserId);

                IdentityResult identityResult =
                    _userManager.ConfirmEmailAsync(identityUser, userDto.CodeActive).Result;

                if (identityResult.Succeeded)
                    return Result.Ok();
                else
                    return Result.Fail("Operation active user failed");
            }
            catch (Exception ex)
            {
                CreateLogDto log = new CreateLogDto
                {
                    Description = ConstMessages.ErroAtivarUsuario,
                    Exception = ex.Message,
                    Place = "RegisterService"
                };

                await _logService.RegisterLog(log);

                return Result.Fail("Operation active user failed");
            }
        } 
    }
}
