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

    public async Task<IActionResult> GetTests(byte ticketId, int testId = 0, string language = null)
    {
        var ticket = new Ticket() { Id = ticketId };

        if (!string.IsNullOrEmpty(language))
        {

            testService.ChangeLanguage(language);
        }
        if (testId == 0)
        {
            testId = ticket.StartIndex;
        }

        var tests = (await testService.ReadFromFile())
            .Where(t => t.Id >= ticket.StartIndex && t.Id <= ticket.EndIndex)
            .ToList();

        var test = tests.Find(t => t.Id == testId);
        ViewBag.TicketId = ticket.Id;
        ViewBag.Ticket = ticket;
        ViewBag.Context = HttpContext;
        ViewBag.Tests = tests;

        return View(test);
    }

    [HttpPost]
    public async Task<IActionResult> GetTestsPost(byte ticketId = 0, int testId = 0, int choiceId = 0)
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

        return RedirectToAction("GetTests", new { ticketId = ticketId, testId = testId});
    }

    public IActionResult Tickets()
    {
        var tickets = new List<Ticket>();
        return View(tickets);
    }

    [HttpPost]
    public IActionResult Tickets(byte id)
    {
        var ticket = new Ticket { Id = id };

        DeleteCookies(ticket);

        return RedirectToAction("GetTests", new { ticketId = id, testId = 0 });
    }

    public IActionResult TestResult(byte ticketId)
    {
        var correctAnswerCount = GetCorrectAnswersCount();
        ViewBag.Count = correctAnswerCount;

        var ticket = new Ticket { Id = ticketId };
        DeleteCookies(ticket);
        return View();
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

    private void DeleteCookies(string key)
    {
        var check = CheckCookie(key);
        if (check)
        {
            HttpContext.Response.Cookies.Delete(key);
        }
    }

    private void DeleteCookies(Ticket ticket)
    {
        for (int i = ticket.StartIndex; i <= ticket.EndIndex; i++)
        {
            DeleteCookies(i.ToString());
            if (i == ticket.StartIndex)
            {
                DeleteCookies(CorrectAnswersCount);
            }
        }
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

    public IActionResult GetPath()
    {
        var path = testService.GetPath();
        ViewBag.Path = path;
        return View();
    }

}
