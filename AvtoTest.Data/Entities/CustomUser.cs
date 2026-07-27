
using System.ComponentModel.DataAnnotations;

namespace AvtoTest.Data.Entities;

public class CustomUser
{
    [Key]
    public Guid Userid { get; set; }
    public string PhotoUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
