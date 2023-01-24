using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Latihan.Services;
using Microsoft.IdentityModel.Tokens;

namespace Latihan.Utils;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IUserService userService)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
            AttachUserToContext(context, userService, token);

        await _next(context);
    }

    private async void AttachUserToContext(HttpContext context, IUserService userService, string token)
    {
        try
        {
            var secretKey = Environment.GetEnvironmentVariable("SECRET_KEY");
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey!);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

            context.Items["User"] = await userService.GetUserById(userId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}