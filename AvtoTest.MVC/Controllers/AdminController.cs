using AvtoTest.Data.Entities.TestEntities;
using Microsoft.AspNetCore.Mvc;

namespace AvtoTest.MVC.Controllers;

public class AdminController : Controller
{
    public IActionResult CreateTest()
    {
        var model = new CreateTest
        {
            Choices = new List<string> { "", "", "", "" }, // 4 ta bo'sh variant
            CorrectChoiceIndex = -1
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult CreateTest(CreateTest createTest)
    { 

    }
}
