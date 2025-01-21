using System.ComponentModel.DataAnnotations;

namespace QuartzSetup.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    [MaxLength(64)]
    public string Username { get; set; } = default!;
    [MaxLength(100)]
    public string Name { get; set; } = default!;
    [MaxLength(32)]
    public string PhoneNumber { get; set; } = default!;
    [EmailAddress]
    [MaxLength(256)]
    public string Email { get; set; } = default!;
    public bool IsLogged { get; set; }
    [MaxLength(255)]
    public string? Role { get; set; } = default!;// UserRole
    public DateTime? LoginAt { get; set; }
    public DateTime? LastActivity { get; set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
}
