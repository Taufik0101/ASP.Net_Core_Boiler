using Latihan.Models;

namespace Latihan.DTO;

public class AuthenticateResponse
{
    public User User { get; set; }
    
    public string Token { get; set; }
}