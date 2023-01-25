using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace KeyVault.Tools
{
    public class TokenTools
    {
        public string GenerateJwt(string secret, string issuer, string audience)
        {
            // create a claims object with the user's information
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "username"),
                new Claim(ClaimTypes.Role, "user")
            };

            // create a symmetric security key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            // create a signing credentials object
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // create a JWT security token
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            // return the token as a string
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }
    }
}