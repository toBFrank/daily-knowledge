namespace DailyKnowledge.Api.Models;

public class Option
{
  public long Id { get; set; }
  public long QuestionId { get; set; }
  public string? OptionText { get; set; }
  public bool IsCorrect { get; set; }

  // Navigation properties
  public Question Question { get; set; } = null!
}