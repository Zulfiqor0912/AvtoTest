using AvtoTest.Data.Entities.TestEntities;
using AvtoTest.Data.Repositories.Interfaces;
using AvtoTest.Service.Services.Interfece;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace AvtoTest.Service.Services;

public class TestService : ITestService
{
    private readonly ITestRepository testRepository;
    private readonly IHomeRepasitory homeRepasitory;
    public List<Test> Tests { get; set; }
    public TestService(ITestRepository testRepository, IHomeRepasitory homeRepasitory)
    {
        this.testRepository = testRepository;
        this.homeRepasitory = homeRepasitory;
        Tests = testRepository.ReadFromFile();
    }

    public List<Test> ReadFromFile()
    {
        var tests = testRepository.ReadFromFile();
        return tests;
    }

    public void ChangeLanguage(string language, HttpContext httpContext)
    {

        if (!string.IsNullOrEmpty(language))
        {
            AddCookies("language", language, httpContext);
        }
        else
        {
            language = GetCookie("language", httpContext);
        }


        Tests = testRepository.ReadFromFile(language);
    }

    public string GetPath()
    {
        var path = testRepository.GetPath();
        return path;
    }

    public Tuple<Test, List<Test>> GetSortedTest(ushort startIndex, ushort endIndex, int testId)
    {
        var tests = Tests
          .Where(t => t.Id >= startIndex && t.Id <= endIndex)
          .ToList();

        var test = tests.Find(t => t.Id == testId);
        return new(test, tests);
    }

    public void AddCookies(string key, string value, HttpContext httpContext)
    {
        var check = CheckCookie(key, httpContext);
        if (!check)
        {
            httpContext.Response.Cookies.Delete(key);
        }
        httpContext.Response.Cookies.Append(key, value);
    }

    public string GetCookie(string key, HttpContext httpContext)
    {
        string value = httpContext.Request.Cookies[key]!;
        if (string.IsNullOrEmpty(value))
            return string.Empty;
        return value;
    }

    private bool CheckCookie(string key, HttpContext httpContext)
    {
        var value = httpContext.Request.Cookies[key];
        if (string.IsNullOrEmpty(value)) return true;
        else return false;
    }

    public Tuple<Ticket, int> GetTicketAndTestId(byte ticketId, int testId)
    {
        var ticket = new Ticket() { Id = ticketId };

        if (testId == 0)
            testId = ticket.StartIndex;

        return new(ticket, testId);
    }

    public bool CheckAnonymousUser()
    {
        throw new NotImplementedException();
    }
}
