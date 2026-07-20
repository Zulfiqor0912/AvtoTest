using AvtoTest.Data.Entities.TestEntities;
using AvtoTest.Data.Repositories.Interfaces;
using AvtoTest.Service.Services.Interfece;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvtoTest.Service.Services;

public class TestService : ITestService
{
    private readonly ITestRepository testRepository;
    public List<Test> Tests { get; set; }
    public TestService(ITestRepository testRepository)
    {
        this.testRepository = testRepository;
        Tests = testRepository.ReadFromFile();
    }

    public List<Test> ReadFromFile()
    {
        var tests = testRepository.ReadFromFile();
        return tests;
    }

    public void ChangeLanguage(string language)
    {
        Tests = testRepository.ReadFromFile(language);
    }

    public string GetPath()
    {
        var path = testRepository.GetPath();
        return path;
    }
}
