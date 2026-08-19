using AvtoTest.Data.Entities.TestEntities;
using AvtoTest.Data.Entities.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace AvtoTest.MVC.Controllers;

public class AdminController(IWebHostEnvironment environment) : Controller
{
    public IActionResult CreateTest()
    {
        var model = new CreateTestViewModel();
        return View(model);
    }

    [HttpPost]
    public IActionResult CreateTest(CreateTestViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var lotinTest = MapForEntity(model.Latin, model.CorrectChoiceIndex);
        var kirilTest = MapForEntity(model.Cyrillic, model.CorrectChoiceIndex);
        var rusTest = MapForEntity(model.Russian, model.CorrectChoiceIndex);
        ReadAndAddTest(lotinTest, kirilTest, rusTest, model.Image);

        return View(model);
    }

    private Test MapForEntity(Test test, int? correctChoiceIndex)
    {
        var model = new Test
        {
            Question = test.Question,
            Description = test.Description,
            Choices = test.Choices.Select((choice, index) => new Choice
            {
                Text = choice.Text,
                Answer = correctChoiceIndex == index
            }).ToList(),
            Media = new Media()
        };
        return model;
    }
    private async Task ReadAndAddTest(Test lotin, Test kiril, Test rus, IFormFile image)
    {
        string rusJson = System.IO.File.ReadAllText(@"./wwwroot/AvtoTest/rus.json");
        string lotinJson = System.IO.File.ReadAllText(@"./wwwroot/AvtoTest/uzlotin.json");
        string kirilJson = System.IO.File.ReadAllText(@"./wwwroot/AvtoTest/uzkiril.json");

        var settings = new JsonSerializerSettings
        {
            ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver
            {
                NamingStrategy = new Newtonsoft.Json.Serialization.DefaultNamingStrategy
                {
                    OverrideSpecifiedNames = false,
                    ProcessDictionaryKeys = false,
                    ProcessExtensionDataNames = false
                }
            },

            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            Error = (sender, args) =>
            {
                args.ErrorContext.Handled = true;
            }
        };

        List<Test> lotinTests = JsonConvert.DeserializeObject<List<Test>>(lotinJson, settings);
        List<Test> kirilTests = JsonConvert.DeserializeObject<List<Test>>(kirilJson, settings);
        List<Test> rusTests = JsonConvert.DeserializeObject<List<Test>>(rusJson, settings);

        var Id = lotinTests.Count + 1;
        lotin.Id = Id;
        kiril.Id = Id;
        rus.Id = Id;

        if (image != null && image.Length > 0)
        {
            var folder = Path.Combine(environment.WebRootPath, "AvtoTest");
            Directory.CreateDirectory(folder);
            var fileName = $"{Id}.png";
            var filePath = Path.Combine(folder, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await image.CopyToAsync(stream);
            
            lotin.Media.Name = Id.ToString();
            lotin.Media.Exist = true;

            kiril.Media.Name = Id.ToString();
            kiril.Media.Exist = true;

            rus.Media.Name = Id.ToString();
            rus.Media.Exist = true;
        }
        
        lotinTests.Add(lotin);
        kirilTests.Add(kiril);
        rusTests.Add(rus);

        var newLotinJson = JsonConvert.SerializeObject(lotinTests, new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
        });
        var newKirilJson = JsonConvert.SerializeObject(kirilTests, new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
        });
        var newRusJson = JsonConvert.SerializeObject(rusTests, new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
        });

        System.IO.File.WriteAllText(@"./wwwroot/AvtoTest/uzlotin.json", newLotinJson);
        System.IO.File.WriteAllText(@"./wwwroot/AvtoTest/uzkiril.json", newKirilJson);
        System.IO.File.WriteAllText(@"./wwwroot/AvtoTest/rus.json", newRusJson);
    }
}
