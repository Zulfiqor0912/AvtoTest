using AvtoTest.Data.Entities.TestEntities;
using AvtoTest.Service.Services.Interfece;
using Microsoft.AspNetCore.Mvc;

namespace AvtoTest.MVC.Controllers;

public class TestController : Controller
{
    private readonly ITestService testService;
    private const string CorrectAnswersCount = "CorrectAnswersCount";

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
        int count = GetCorrectAnswersCount();

        var ticket = new Ticket() { Id = ticketId };
        var test = (await testService.ReadFromFile()).Find(t => t.Id == testId);
        if (test.Choices[choiceId].Answer)
        {
            count++;
        }
        if (testId != 0)
        {
            AddCookies(testId.ToString(), choiceId.ToString());
            AddCookies(CorrectAnswersCount, count.ToString());
        }

        return RedirectToAction("GetTests", ticket);
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

    private void AddCookies(string key, string value)
    {
        var check = CheckCookie(key);
        if (!check)
        {
            HttpContext.Response.Cookies.Delete(key);
        }
        HttpContext.Response.Cookies.Append(key, value);
    }

    private bool CheckCookie(string key)
    {
        var value = HttpContext.Request.Cookies[key];
        if (string.IsNullOrEmpty(value)) return true;
        else return false;
    }

    private int GetCorrectAnswersCount()
    {
        string correctAnswersCount = HttpContext.Request.Cookies["correctAnswersCount"];
        int count = 0;
        count = string.IsNullOrEmpty(correctAnswersCount) ? 0 : Convert.ToInt32(correctAnswersCount);
        return count;
    }

}
