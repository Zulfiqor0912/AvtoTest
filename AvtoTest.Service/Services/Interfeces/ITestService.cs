using AvtoTest.Data.Entities.TestEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvtoTest.Service.Services.Interfece;

public interface ITestService
{
    public Task<List<Test>> ReadFromFile();

    public string GetPath();

    public Task ChangeLanguage(string language);
}
