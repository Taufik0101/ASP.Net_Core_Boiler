using Latihan.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Latihan.DTO;

public struct Register
{
    public string Email { get; set; }
    
    public string Name { get; set; }

    public string Password { get; set; }
    
    public Role Role { get; set; }
}