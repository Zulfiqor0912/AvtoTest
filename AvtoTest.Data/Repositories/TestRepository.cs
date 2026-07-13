using AvtoTest.Data.Entities.TestEntities;
using AvtoTest.Data.Repositories.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvtoTest.Data.Repositories;

public class TestRepository : ITestRepository
{
    private string Path { get; set; }
    public async Task<List<Test>> GetAllTests(string language)
    {
        if (string.IsNullOrEmpty(language)) language = "uzb";
        var tests = new List<Test>();
        switch  (language)
        {
            case "uzb":
                Path = "uzlotin.json";
                break;
            case "rus":
                Path = "rus.json";
                break;
            case "kiril":
                Path = "uzkiril.json";
                break;
            default:
                Path = "uzlotin.json";
                break;
        }
        var json = await File.ReadAllTextAsync(Path);
        tests = JsonConvert.DeserializeObject<List<Test>>(json);
        return tests;
    }


}   
