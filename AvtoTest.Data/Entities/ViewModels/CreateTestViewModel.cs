using AvtoTest.Data.Entities.TestEntities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvtoTest.Data.Entities.ViewModels;

public class CreateTestViewModel
{
    public Test Latin { get; set; } = CreateEmptyTest();

    public Test Cyrillic { get; set; } = CreateEmptyTest();

    public Test Russian { get; set; } = CreateEmptyTest();

    // Uchala til uchun bitta umumiy to'g'ri javob.
    // Masalan 0 = A, 1 = B, 2 = C, 3 = D.
    public int? CorrectChoiceIndex { get; set; }

    // Rasm uchala til uchun umumiy.
    public IFormFile? Image { get; set; }

    private static Test CreateEmptyTest()
    {
        return new Test
        {
            Question = string.Empty,
            Description = string.Empty,
            Choices = new List<Choice>
            {
                new Choice(),
                new Choice(),
                new Choice(),
                new Choice()
            },
            Media = new Media()
        };
    }
}
