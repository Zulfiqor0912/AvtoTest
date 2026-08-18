using Microsoft.AspNetCore.Mvc;

namespace AvtoTest.MVC.Controllers;

public class AdminController : Controller
{
    public IActionResult CreateTest()
    {
        return View();
    }
}
