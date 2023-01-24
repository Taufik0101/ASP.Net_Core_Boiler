using Latihan.DTO;
using Latihan.Models;
using Latihan.Utils;

namespace Latihan.Services;

public interface IAuthService
{
    Task<AuthenticateResponse> Login(LoginRequest input);

    Task<User> Register(Register input);
}

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly IHash _hash;
    private readonly IJwtToken _jwtToken;

    public AuthService(IUserService userService, IHash hash, IJwtToken jwtToken)
    {
        _userService = userService;
        _hash = hash;
        _jwtToken = jwtToken;
    }
    
    public async Task<AuthenticateResponse> Login(LoginRequest input)
    {
        var validEmail = _userService.ValidateEmail(input.Email);
        if (!validEmail)
        {
            throw new ApplicationException("Email Not Valid");
        }

        var user = await _userService.GetUserByEmail(input.Email);
        
        if (user == null)
        {
            throw new BadHttpRequestException("Email Or Password Doesn't Match");
        }

        var res = await _hash.VerifyPassword(new ParamsVerifyPassword()
        {
            Password = input.Password,
            Hashed = user.Password
        });

        if (!res.Verify)
        {
            throw new BadHttpRequestException("Email Or Password Doesn't Match");
        }
        
        var token = await _jwtToken.GenerateToken(user);

        var authRes = new AuthenticateResponse()
        {
            User = user,
            Token = token
        };

        return authRes;
    }

    public async Task<User> Register(Register input)
    {
        var validEmail = _userService.ValidateEmail(input.Email);
        if (!validEmail)
        {
            throw new ApplicationException("Email Not Valid");
        }
        
        var userEmail = await _userService.GetUserByEmail(input.Email);

        if (userEmail != null)
        {
            throw new BadHttpRequestException("Email Already Used");
        }
        
        var password = await _hash.HashPassword(input.Password);

        input.Password = password;
        
        var user = await _userService.CreateUser(input);

        return user;
    }
}