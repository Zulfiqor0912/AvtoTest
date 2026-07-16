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

    public async Task<IActionResult> GetTests(Ticket ticket)
    {
        var tests = (await testService.ReadFromFile())
            .Where(t => t.Id >= ticket.StartIndex && t.Id <= ticket.EndIndex)
            .ToList();

        ViewBag.TicketId = ticket.Id;

        ViewBag.Context = HttpContext;
        return View(tests);
    }

    [HttpPost]
    public async Task<IActionResult> GetTests(byte ticketId = 0, int testId = 0, int choiceId = 0)
    {
        var ticket = new Ticket() { Id = ticketId };
        if (testId != 0)
        {
            HttpContext.Response.Cookies.Append(testId.ToString(), choiceId.ToString());
        }

        return RedirectToAction("Tickets", ticket);
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
