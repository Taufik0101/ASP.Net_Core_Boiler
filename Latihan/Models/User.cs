using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Latihan.Models;

public class User
{
    [Column("id")]
    public Guid Id { get; set; }
    
    [Required]
    [Column("email")]
    [MaxLength(100), EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [JsonIgnore]
    [Column("password")]
    [MaxLength(255)]
    public string Password { get; set; }
    
    [Required]
    [Column("name")]
    [MaxLength(255)]
    public string Name { get; set; }
    
    [Required]
    [Column("role")]
    public Role Role { get; set; }
    
    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }
    
    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }
}