using AvtoTest.Data.Entities.TestEntities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvtoTest.Service.Services.Interfece;

public interface ITestService
{
    public List<Test> ReadFromFile();

    public string GetPath();

    public void ChangeLanguage(string language, HttpContext httpContext);
    public bool CheckAnonymousUser();
}
