using FluentResults;
using Microsoft.AspNetCore.Mvc;
using SistemaLogin.Dtos.Log;
using SistemaLogin.Dtos.User;
using SistemaLogin.Helpers;
using SistemaLogin.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaLogin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    { 
        private readonly LoginService _loginService;
        private readonly LogService _logService;

        public LoginController(LoginService loginService, LogService logService)
        {
            _loginService = loginService;
            _logService = logService;
        }

        [HttpPost]
        public async Task<IActionResult> LoginTo(LoginDto loginDto)
        {
            try
            {
                Result result = await _loginService.LoginTo(loginDto);

                if (result.IsFailed)
                {
                    return StatusCode(ConstMessages.StatusUnauthorized401,
                            new { Message = "Operation login failed" });
                }

                string token = result.Successes.FirstOrDefault().Message;

                return StatusCode(ConstMessages.StatusOK200,
                    new { Message = "Success", Token = token });
            }
            catch (Exception ex)
            {

                CreateLogDto log = new CreateLogDto
                {
                    Description = ConstMessages.ErroFazerLogin,
                    Exception = ex.Message,
                    Place = "LoginController"
                };

                await _logService.RegisterLog(log);

                return StatusCode(ConstMessages.StatusUnauthorized401,
                    new { Message = "Operation login failed" });
            }
        }
    
        [HttpPost("/RequestResetPassword")]
        public async Task<IActionResult> RequestResetPassword(RequestResetPasswordDto dto)
        {
            try
            {
                Result result = await _loginService.RequestResetPassword(dto);

                if (result.IsFailed)
                    return StatusCode(ConstMessages.StatusUnauthorized401,
                        new { Message = "Failed operation generate token reset password" });

                string tokenReset = result.Successes.FirstOrDefault().Message;

                return StatusCode(ConstMessages.StatusOK200,
                        new { Message = "Token reset password generated", tokenReset = tokenReset });
            }
            catch (Exception ex)
            {
                CreateLogDto log = new CreateLogDto
                {
                    Description = ConstMessages.ErroGerarTokenRedefinirSenha,
                    Exception = ex.Message,
                    Place = "LoginController"
                };

                await _logService.RegisterLog(log);

                return StatusCode(ConstMessages.StatusInternalServerError500,
                        new { Message = "Failed operation generate token reset password" });
            }
        }

        [HttpPost("/ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            try
            {
                Result result = await _loginService.ResetPassword(dto);

                if (result.IsFailed)
                    return StatusCode(ConstMessages.StatusUnauthorized401,
                        new { Message = "Reset password failed" });

                return StatusCode(ConstMessages.StatusOK200,
                    new { Message = "Password reseted successfully" });
            }
            catch (Exception ex)
            {
                CreateLogDto log = new CreateLogDto
                {
                    Description = ConstMessages.ErroAoRedefinirSenha,
                    Exception = ex.Message,
                    Place = "LoginController"
                };

                await _logService.RegisterLog(log);

                return StatusCode(ConstMessages.StatusInternalServerError500,
                        new { Message = "Error reset password" });
            }
        }
    }
}