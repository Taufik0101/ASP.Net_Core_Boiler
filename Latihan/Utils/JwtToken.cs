using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Latihan.Models;
using Microsoft.IdentityModel.Tokens;

namespace Latihan.Utils;

public interface IJwtToken
{
    Task<string> GenerateToken(User user);
    Task<string> ValidateToken(string token);
}

public class JwtToken : IJwtToken
{
    public Task<string> GenerateToken(User user)
    {
        var secretKey = Environment.GetEnvironmentVariable("SECRET_KEY");
        // generate token that is valid for 7 days
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretKey!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("id", user.Id.ToString()),
            }),
            Expires = DateTime.UtcNow.AddMonths(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Task.FromResult(tokenHandler.WriteToken(token));
    }
    public Task<string> ValidateToken(string token)
    {
        if (token == "") 
            throw new BadHttpRequestException("Token Not Found");

        var secretKey = Environment.GetEnvironmentVariable("SECRET_KEY");
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretKey!);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = jwtToken.Claims.First(x => x.Type == "id").Value;

            // return user id from JWT token if validation successful
            return Task.FromResult(userId);
        }
        catch
        {
            // return null if validation fails
            throw new BadHttpRequestException("Validation Fails");
        }
    }
}