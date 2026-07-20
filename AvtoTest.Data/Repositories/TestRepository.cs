using AvtoTest.Data.Entities.TestEntities;
using AvtoTest.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvtoTest.Data.Repositories;

public class TestRepository : ITestRepository
{
    private readonly IHostingEnvironment environment;
    private string Path => environment.WebRootPath + "\\AvtoTest\\";

    public TestRepository(IHostingEnvironment environment)
    {
        this.environment = environment;
    }
    public List<Test> GetAllTests(string? language = null)
    {
        string filePath = Path;
        if (string.IsNullOrEmpty(language)) language = "uzb";

        switch  (language)
        {
            case "uzb":
                filePath += "uzlotin.json";
                break;
            case "rus":
                filePath += "rus.json";
                break;
            case "kiril":
                filePath += "uzkiril.json";
                break;
        }
        var json = File.ReadAllText(filePath);
        List<Test> tests = JsonConvert.DeserializeObject<List<Test>>(json)!;
        return tests;
    }

    public string GetPath()
    {
        var path = environment.WebRootPath;
        var m = path + "\\AvtoTest\\uzlotin.json";
        return path;
    }
}   
