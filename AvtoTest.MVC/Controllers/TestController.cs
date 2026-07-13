using AvtoTest.Data.Entities.TestEntities;
using AvtoTest.Service.Services.Interfece;
using Microsoft.AspNetCore.Mvc;

namespace AvtoTest.MVC.Controllers;

public class TestController : Controller
{
    private readonly ITestService testService;

    public TestController(ITestService testService)
    {
        this.testService = testService;
    }
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> GetTests()
    {
        var tests = (await testService.ReadFromFile()).Where(t => t.Id <= 19).ToList();
        return View(tests);
    }

    public IActionResult Tickets()
    {
        var tickets = new List<Ticket>();
        return View(tickets);
    }

    [HttpPost]
    public IActionResult Tickets(byte id)
    {
        var ticket = new Ticket() { Id = id };
        return RedirectToAction("GetTests", ticket);
    }

}
