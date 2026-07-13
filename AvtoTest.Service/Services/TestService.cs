using AvtoTest.Data.Entities.TestEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvtoTest.Service.Services;

public class TestService
{
    public List<Test> Tests { get; set; }
    public TestService()
    {
        Tests = new List<Test>();
    }
}
