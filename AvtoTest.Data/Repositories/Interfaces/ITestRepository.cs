using AvtoTest.Data.Entities.TestEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvtoTest.Data.Repositories.Interfaces;

public interface ITestRepository
{
    public List<Test> ReadFromFile(string? language = null);

    public string GetPath();
}
