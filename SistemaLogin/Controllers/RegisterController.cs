using FluentResults;
using Microsoft.AspNetCore.Mvc;
using SistemaLogin.Dtos.Log;
using SistemaLogin.Dtos.User;
using SistemaLogin.Helpers;
using SistemaLogin.Services;
using System;
using System.Threading.Tasks;

namespace SistemaLogin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly RegisterService _registerService;
        private readonly LogService _logService;

        public RegisterController(RegisterService registerService, LogService logService)
        {
            _registerService = registerService;
            _logService = logService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] CreateUserDto userDto)
        {
            try
            {
                Result result = await _registerService.CreateUser(userDto);

                if (result.IsFailed)
                    return StatusCode(ConstMessages.StatusInternalServerError500, 
                        new { Message = "Operation create user failed" });

                return StatusCode(ConstMessages.StatusOK200, 
                    new { Message = "User created successfully" });
            }
            catch (Exception ex)
            {
                CreateLogDto log = new CreateLogDto
                {
                    Description = ConstMessages.ErroCriarUsuario,
                    Exception = ex.Message,
                    Place = "RegisterController"
                };

                await _logService.RegisterLog(log);

                return StatusCode(ConstMessages.StatusInternalServerError500, 
                    new { Message = "Operation create user failed" });
            }
        } 

        [HttpGet("/Register/Active")]
        public async Task<IActionResult> ActiveRegisterUser([FromQuery] ActiveUserDto userDto)
        {
            try
            {
                Result result = await _registerService.ActiveUser(userDto);

                if (result.IsFailed)
                    return StatusCode(ConstMessages.StatusInternalServerError500,
                        new { Message = "Active user failed" });
                else
                    return StatusCode(ConstMessages.StatusOK200,
                        new { Message = "User confirmed successfully" });
            }
            catch (Exception ex)
            {
                CreateLogDto log = new CreateLogDto
                {
                    Description = ConstMessages.ErroAtivarUsuario,
                    Exception = ex.Message,
                    Place = "RegisterController"
                };

                await _logService.RegisterLog(log);

                return StatusCode(ConstMessages.StatusInternalServerError500,
                        new { Message = "Active user failed" });
            }
        }
    }
}