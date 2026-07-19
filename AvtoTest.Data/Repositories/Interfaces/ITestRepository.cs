using AvtoTest.Data.Entities.TestEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvtoTest.Data.Repositories.Interfaces;

public interface ITestRepository
{
    public Task<List<Test>> GetAllTests(string? language = null);

    public string GetPath();
}
