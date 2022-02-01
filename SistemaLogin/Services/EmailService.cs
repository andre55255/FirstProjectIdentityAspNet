using FluentResults;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using SistemaLogin.Dtos.Log;
using SistemaLogin.Helpers;
using SistemaLogin.Models;
using System;
using System.Threading.Tasks;

namespace SistemaLogin.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;
        private readonly LogService _logService;

        public EmailService(IConfiguration configuration, LogService logService)
        {
            _configuration = configuration;
            _logService = logService;
        }

        public async Task<Result> SenderEmail(string[] recipients, 
            string subject, 
            int userId, 
            string codeConfirmation)
        {
            try
            {
                Message message = new Message(recipients, subject, userId, codeConfirmation);

                MimeMessage msgEmail = await CreateBodyEmail(message);

                await Sender(msgEmail);

                return Result.Ok();
            }
            catch (Exception ex)
            {
                CreateLogDto log = new CreateLogDto
                {
                    Description = ConstMessages.ErroEnviarEmail,
                    Exception = ex.Message,
                    Place = "EmailService"
                };

                await _logService.RegisterLog(log);

                return Result.Fail("Send email failed");
            }
        }

        private async Task Sender(MimeMessage msgEmail)
        {
            using (SmtpClient client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_configuration.GetValue<string>("EmailSettings:SmtpServer"),
                        _configuration.GetValue<int>("EmailSettings:Port"));

                    client.AuthenticationMechanisms.Remove("XOAUTH2");

                    await client.AuthenticateAsync(_configuration.GetValue<string>("EmailSettings:From"),
                        _configuration.GetValue<string>("EmailSettings:Password"));

                    await client.SendAsync(msgEmail);
                }
                catch (Exception ex)
                {
                    CreateLogDto log = new CreateLogDto
                    {
                        Description = ConstMessages.ErroEnviarEmail,
                        Exception = ex.Message,
                        Place = "EmailService"
                    };

                    await _logService.RegisterLog(log);
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }

        private async Task<MimeMessage> CreateBodyEmail(Message message)
        {
            try
            {
                MimeMessage messageEmail = new MimeMessage();
                messageEmail.From.Add(new MailboxAddress(_configuration.GetValue<string>("EmailSettings:From")));
                messageEmail.To.AddRange(message.Recipients);
                messageEmail.Body = new TextPart(MimeKit.Text.TextFormat.Text)
                {
                    Text = message.Content
                };

                return messageEmail;
            }
            catch (Exception ex)
            {
                CreateLogDto log = new CreateLogDto
                {
                    Description = ConstMessages.ErroEnviarEmail,
                    Exception = ex.Message,
                    Place = "EmailService"
                };

                await _logService.RegisterLog(log);

                return null;
            }
        }
    }
}