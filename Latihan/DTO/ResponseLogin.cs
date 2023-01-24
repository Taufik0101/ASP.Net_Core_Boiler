namespace Latihan.DTO;

public struct ResponseLogin
{
    public Guid Id { get; set; }
    
    public string Email { get; set; }
    
    public string Name { get; set; }
    
    public string Token { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    
    public DateTime? DeletedAt { get; set; }
}