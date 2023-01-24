namespace Latihan.DTO;

public struct ParamsVerifyPassword
{
    public string Hashed { get; set; }
    
    public string Password { get; set; }
}