using Microsoft.AspNetCore.Identity;

namespace AvtoTest.Data.Entities;

public class CustomUser : IdentityUser
{
    public string PhotoUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
