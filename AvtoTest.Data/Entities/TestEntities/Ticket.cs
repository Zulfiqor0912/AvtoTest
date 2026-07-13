using System;
using System.Collections.Generic;
using System.Text;

namespace AvtoTest.Data.Entities.TestEntities;

public class Ticket
{
    public byte Id { get; set; }
    public ushort StartIndex => (ushort)((Id - 1) * 20 + 1);
    public ushort EndIndex => (ushort)(Id * 20);
}
