using Microsoft.AspNetCore.Identity;

namespace AvtoTest.Data.Entities;

public class CustomUser : IdentityUser
{
    public string PhotoUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
