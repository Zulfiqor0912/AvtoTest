
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AvtoTest.Data.Entities;

public class CustomUser : IdentityUser
{
    public string PhotoUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public List<Result> Results { get; set; }
}
