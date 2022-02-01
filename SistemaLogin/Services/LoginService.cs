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

namespace SistemaLogin.Services
{
    public class LoginService
    {
        private readonly SignInManager<IdentityUser<int>> _signInManager;
        private readonly TokenService _tokenService;
        private readonly LogService _logService;

        public LoginService(SignInManager<IdentityUser<int>> signInManager,
            TokenService tokenService,
            LogService logService)
        {
            _signInManager = signInManager;
            _tokenService = tokenService;
            _logService = logService;
        }

        public async Task<Result> LoginTo(LoginDto loginDto)
        {
            try
            {
                SignInResult resultIdentity =
                    await _signInManager
                                  .PasswordSignInAsync(
                                        loginDto.Username,
                                        loginDto.Password,
                                        false,
                                        false
                                  );

                if (resultIdentity.Succeeded)
                {
                    IdentityUser<int> userIdentity =
                        await _signInManager
                                    .UserManager
                                    .Users
                                    .FirstOrDefaultAsync(us =>
                                        us.NormalizedUserName == loginDto.Username.ToUpper()
                                    );

                    Token token = _tokenService.CreateToken(userIdentity);

                    return Result.Ok().WithSuccess(token.Value);
                }

                return Result.Fail("Login failed");
            }
            catch (Exception ex)
            {
                CreateLogDto log = new CreateLogDto
                {
                    Description = ConstMessages.ErroFazerLogin,
                    Exception = ex.Message,
                    Place = "LoginService"
                };

                await _logService.RegisterLog(log);

                return Result.Fail("Login failed");
            }
        }

        public async Task<Result> RequestResetPassword(RequestResetPasswordDto dto)
        {
            try
            {
                IdentityUser<int> identityUser = await GetUserByEmail(dto.Email);

                if (identityUser != null)
                {
                    string tokenReset =
                        await _signInManager
                                    .UserManager
                                    .GeneratePasswordResetTokenAsync(identityUser);

                    return Result.Ok().WithSuccess(tokenReset);
                }
                else
                {
                    return Result.Fail("Operation is failed");
                }
            }
            catch (Exception ex)
            {
                CreateLogDto log = new CreateLogDto
                {
                    Description = ConstMessages.ErroGerarTokenRedefinirSenha,
                    Exception = ex.Message,
                    Place = "LoginService"
                };

                await _logService.RegisterLog(log);

                return Result.Fail("Operation is failed");
            }
        }

        public async Task<Result> ResetPassword(ResetPasswordDto dto)
        {
            try
            {
                IdentityUser<int> identityUser = await GetUserByEmail(dto.Email);

                IdentityResult result =
                    await _signInManager
                               .UserManager
                               .ResetPasswordAsync(identityUser, dto.TokenReset, dto.Password);

                if (result.Succeeded)
                {
                    return Result.Ok();
                }
                else
                {
                    return Result.Fail("Reset password failed");
                }
            }
            catch (Exception ex)
            {
                CreateLogDto log = new CreateLogDto
                {
                    Description = ConstMessages.ErroAoRedefinirSenha,
                    Exception = ex.Message,
                    Place = "LoginService"
                };

                await _logService.RegisterLog(log);

                return Result.Fail("Reset password failed");
            }
        }

        private async Task<IdentityUser<int>> GetUserByEmail(string email)
        {
            try
            {
                IdentityUser<int> identityUser =
                    await _signInManager
                                .UserManager
                                .Users
                                .FirstOrDefaultAsync(us =>
                                    us.NormalizedEmail == email.ToUpper()
                                );

                return identityUser;
            }
            catch (Exception ex)
            {
                CreateLogDto log = new CreateLogDto
                {
                    Description = ConstMessages.ErroRecuperarUsuarioEmail,
                    Exception = ex.Message,
                    Place = "LoginService"
                };

                await _logService.RegisterLog(log);

                return null;
            }
        }
    }
}
