using AvtoTest.Data.Context;
using AvtoTest.Data.Entities;
using AvtoTest.Data.Entities.TestEntities;
using AvtoTest.Data.Repositories.Interfaces;
using AvtoTest.Service.Services;
using AvtoTest.Service.Services.Interfece;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Sockets;
using static System.Net.Mime.MediaTypeNames;

namespace AvtoTest.MVC.Controllers;

//[Authorize]
public class TestController : Controller
{
    private readonly TestService _testService;
    private const string CorrectAnswersCount = "CorrectAnswersCount";
    private readonly UserManager<CustomUser> _userManager;
    private readonly IResultRepository _resultRepository;

    public TestController(
        TestService testService, 
        UserManager<CustomUser> userManager, 
        IResultRepository resultRepository)
    {
        _testService = testService;
        _userManager = userManager;
        _resultRepository = resultRepository;
    }
    public IActionResult Index()
    {
        return View();
    }
    public async Task<IActionResult> GetTests(
        byte ticketId, 
        int testId = 0, 
        string language = null, 
        bool retake = false)
    {
        
        var user = await GetUser();

        if (await CheckLogin())
        {
            var result = await _resultRepository.GetResultById(ticketId, user.Id);

            if (result is not null && retake == false)
                return RedirectToAction("Results", result);

            if (retake)
            {
                if (result is not null)
                    await _resultRepository.DeleteResult(result);

                DeleteCookies(new Ticket { Id = ticketId });
                return RedirectToAction("GetTests", new { ticketId, testId = 0 });
            }

            _testService.ChangeLanguage(language, HttpContext);

            (Ticket ticket, testId) = _testService.GetTicketAndTestId(ticketId, testId);

            ViewBag.TicketId = ticket.Id;
            ViewBag.Ticket = ticket;
            ViewBag.Context = HttpContext;

            var (test, tests) = _testService.GetSortedTest(ticket.StartIndex, ticket.EndIndex, testId);

            ViewBag.Tests = tests;

            return View(test);
        }
        else
        {
            
            if (retake)
            {
                DeleteCookies(new Ticket { Id = ticketId });
                return RedirectToAction("GetTests", new { ticketId, testId = 0 });
            }

            _testService.ChangeLanguage(language, HttpContext);

            (Ticket ticket, testId) = _testService.GetTicketAndTestId(ticketId, testId);

            ViewBag.TicketId = ticket.Id;
            ViewBag.Ticket = ticket;
            ViewBag.Context = HttpContext;

            var (test, tests) = _testService.GetSortedTest(ticket.StartIndex, ticket.EndIndex, testId);

            ViewBag.Tests = tests;

            return View(test);
        }
        
    }
    [HttpPost]
    public async Task<IActionResult> GetTestsPost(byte ticketId = 0, int testId = 0, int choiceId = 0)
    {
        var ticket = new Ticket() { Id = ticketId };
        var test = _testService.ReadFromFile().Find(t => t.Id == testId);
        await AddScore(testId, choiceId, test);

        return RedirectToAction("GetTests", new { ticketId = ticketId, testId = testId});
    }
    public async Task<IActionResult> Results(Result result)
    {
        return View(result);
    }
    //[Authorize]
    public IActionResult Tickets()
    {
        var tickets = new List<Ticket>();
        return View(tickets);
    }
    [HttpPost]
    public async Task<IActionResult> Tickets(byte id)
    {
        var ticket = new Ticket { Id = id };

        DeleteCookies(ticket);
        if (await CheckLogin())
        {
            return RedirectToAction("GetTests", new { ticketId = id, testId = 0 });
        }
        else
        {
            if (GetTestsSolvedCount())
                return RedirectToAction("GetTests", new { ticketId = id, testId = 0 });
            else return RedirectToPage("/Account/Login", new { area = "Identity" });
        }
    }
    public async Task<IActionResult> TestResult(byte ticketId)
    {
        var correctAnswerCount = GetCorrectAnswersCount();
        ViewBag.Count = correctAnswerCount;
        
        var ticket =  new Ticket { Id = ticketId };
        var user = await GetUser();

        if (await CheckLogin())
            await _resultRepository.AddResult(ticketId, user.Id, correctAnswerCount);

        ViewBag.TicketId = ticket.Id;
        ViewBag.Ticket = ticket;
        ViewBag.Context = HttpContext;

        var (test, tests) = _testService.GetSortedTest(ticket.StartIndex, ticket.EndIndex, ticket.StartIndex);

        ViewBag.Tests = tests;

        return View(test);
    }
    [HttpPost]
    public async Task<IActionResult> RetakeTest(
        byte ticketId,
        int testId = 0,
        string language = null)
    {
        var user = await GetUser();
        var isLogin = await CheckLogin();

        var ticket = new Ticket { Id = ticketId };

        if (isLogin && user != null)
        {
            var result = await _resultRepository.GetResultById(ticketId, user.Id);
            if (result != null)
                await _resultRepository.DeleteResult(result);
        }
        else
        {
            if (!GetTestsSolvedCount())
            {
                TempData["ErrorMessage"] = "Siz anonim foydalanuvchi sifatida faqat 2 marta test topshira olasiz. Iltimos, tizimga kiring.";
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
        }
        DeleteCookies(new Ticket { Id = ticketId });

        return RedirectToAction("GetTests", new
        {
            ticketId = ticketId,
            testId = testId,
            language = language,
            retake = true
        });
    }
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
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
        HttpContext.Response.Cookies.Delete(key);
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
    private async Task<CustomUser> GetUser()
    {
        var user = await _userManager.GetUserAsync(User);
        return user!;
    }
    private async Task AddScore(int testId, int choiceId, Test test)
    {
        int count = GetCorrectAnswersCount();
        if (test.Choices[choiceId].Answer)
        {
            count++;
        }
        if (testId != 0)
        {
            AddCookies(testId.ToString(), choiceId.ToString());
            AddCookies(CorrectAnswersCount, count.ToString());
        }
    }
    //private async Task<IActionResult> HandleRetryTest(byte ticketId)
    //{
    //    await _resultRepository.DeleteResult
    //}
    private bool GetTestsSolvedCount()
    {
        var json = Request.Cookies["AnonymousUser"];
        AnonymousUser visitor = new AnonymousUser();
        if (json != null)
        {
            visitor = JsonConvert.DeserializeObject<AnonymousUser>(json);
            if (visitor.TestCount == 2) return false;
            visitor.TestCount += 1;
            visitor.LastTestAt = DateTime.UtcNow;
            var setJson = JsonConvert.SerializeObject(visitor);
            Response.Cookies.Append("AnonymousUser", setJson);
        }
        return true;
    }
    private async Task<bool> CheckLogin()
    {
        if (await GetUser() is null) return false;
        else return true;
    }

}