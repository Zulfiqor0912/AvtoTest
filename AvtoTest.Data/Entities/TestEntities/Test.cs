using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace AvtoTest.Data.Entities.TestEntities;

public class Test
{
    public int Id { get; set; }
    public string Question { get; set; }
    public List<Choice> Choices { get; set; }
    public Media Media { get; set; }
    public string Description { get; set; }
}
