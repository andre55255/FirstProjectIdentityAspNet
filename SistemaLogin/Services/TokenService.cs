using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SistemaLogin.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SistemaLogin.Services
{
    public class TokenService
    {
        public Token CreateToken(IdentityUser<int> user)
        {
            // Direitos do usuário, payload do token
            Claim[] rightsUser = new Claim[]
            {
                new Claim("username", user.UserName),
                new Claim("id", user.Id.ToString())
            };

            // Key Secret
            SymmetricSecurityKey key =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes("0b2a216bd6cb16b5fe18f1a8d8"));

            // Gerando as credenciais com base no segredo e no algoritmo de criptografia
            SigningCredentials credentials =
                new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                claims: rightsUser,
                signingCredentials: credentials,
                expires: DateTime.Now.AddHours(1)
            );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new Token(tokenString);
        }
    }
}