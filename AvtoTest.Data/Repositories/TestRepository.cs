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
    public async Task<List<Test>> GetAllTests(string language)
    {
        string filePath = Path;
        if (string.IsNullOrEmpty(language)) language = "uzb";
        var tests = new List<Test>();
        switch  (language)
        {
            case "uzb":
                filePath += "uzlotin.json";
                break;
            case "rus":
                filePath += "rus.json";
                break;
            case "krill":
                filePath += "uzkiril.json";
                break;
            default:
                filePath += "uzlotin.json";
                break;
        }
        var json = await File.ReadAllTextAsync(filePath);
        tests = JsonConvert.DeserializeObject<List<Test>>(json);
        return tests;
    }

    public string GetPath()
    {
        var path = environment.WebRootPath;
        return path;
    }
}   
