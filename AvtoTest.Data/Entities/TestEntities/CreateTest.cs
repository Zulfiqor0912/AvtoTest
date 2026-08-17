using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AvtoTest.Data.Entities.TestEntities;

public class CreateTest
{
    [Required(ErrorMessage = "Savol matnini kiriting")]
    [Display(Name = "Savol matni")]
    public string Question { get; set; } = string.Empty;

    [Display(Name = "Tushuntirish (izoh)")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Kamida 2 ta javob varianti bo'lishi kerak")]
    [MinLength(2, ErrorMessage = "Kamida 2 ta javob varianti bo'lishi kerak")]
    [Display(Name = "Javob variantlari")]
    public List<string> Choices { get; set; } = new();

    [Required(ErrorMessage = "To'g'ri javobni belgilang")]
    [Display(Name = "To'g'ri javob")]
    [Range(0, 10, ErrorMessage = "To'g'ri javobni tanlang")]
    public int CorrectChoiceIndex { get; set; } = -1;

    [Display(Name = "Rasm (ixtiyoriy)")]
    public IFormFile? Image { get; set; }
}
