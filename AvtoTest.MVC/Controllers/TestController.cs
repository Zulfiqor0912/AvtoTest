using AvtoTest.Data.Context;
using AvtoTest.Data.Entities;
using AvtoTest.Data.Entities.TestEntities;
using AvtoTest.Service.Services;
using AvtoTest.Service.Services.Interfece;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AvtoTest.MVC.Controllers;

public class TestController : Controller
{
    private readonly TestService testService;
    private const string CorrectAnswersCount = "CorrectAnswersCount";
    private readonly UserManager<CustomUser> _userManager;
    private readonly AppDbContext _appDbContext;

    public TestController(TestService testService, UserManager<CustomUser> userManager, AppDbContext appDbContext)
    {
        this.testService = testService;
        _userManager = userManager;
        _appDbContext = appDbContext;
    }
    public IActionResult Index()
    {
        return View();
    }
    [Authorize]
    public async Task<IActionResult> GetTests(byte ticketId, int testId = 0, string language = null, bool retake = false)
    {

        var user = await GetUser();
        var result = await _appDbContext.Results.FirstOrDefaultAsync
            (r => r.TicketId == ticketId 
            && r.UserId == user.Id);

        var ticket = new Ticket();

        if (result is not null && retake == false)
        {
            return RedirectToAction("Results", result);
        }

        if (retake && result is not null)
        {
            _appDbContext.Results.Remove(result!);
            await _appDbContext.SaveChangesAsync();
        }

        ticket = new Ticket() { Id = ticketId };

        if (!string.IsNullOrEmpty(language))
        {
            AddCookies("language", language);
        }
        else
        {
            language = GetCookie("language");
        }

        testService.ChangeLanguage(language);

        if (testId == 0)
        {
            testId = ticket.StartIndex;
        }

        var tests = testService.Tests
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
        var test = testService.ReadFromFile().Find(t => t.Id == testId);
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

    public async Task<IActionResult> Results(Result result)
    {
        return View(result);
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

    [Authorize]
    public async Task<IActionResult> TestResult(byte ticketId)
    {
        var correctAnswerCount = GetCorrectAnswersCount();
        ViewBag.Count = correctAnswerCount;

        var ticket = new Ticket { Id = ticketId };
        var user = await GetUser();
        var result = new Result()
        {
            TicketId = ticket.Id,
            CorrectAnswerCount = (byte)correctAnswerCount,
            UserId = user!.Id
        };
        _appDbContext.Results.Add(result);
        await _appDbContext.SaveChangesAsync();
        DeleteCookies(ticket);
        DeleteCookies("language");
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

    private string GetCookie(string key)
    {
        string value = HttpContext.Request.Cookies[key]!;
        if (string.IsNullOrEmpty(value))
            return string.Empty;
        return value;
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

    private async Task<CustomUser> GetUser()
    {
        var user = await _userManager.GetUserAsync(User);
        return user!;
    }

}