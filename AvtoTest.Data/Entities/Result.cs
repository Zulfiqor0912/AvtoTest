using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AvtoTest.Data.Entities;

public class Result
{
    [Key]
    public int Id { get; set; }
    public byte TicketId { get; set; }
    public byte CorrectAnswerCount { get; set; }
    public byte IncorrectAnswerCount => (byte) (TotalAnswersCount - CorrectAnswerCount);
    public const byte TotalAnswersCount = 20;
    public string UserId { get; set; } = string.Empty;
    public CustomUser? CustomUser { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
