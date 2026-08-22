namespace DailyKnowledge.Api.Models;

public class Question
{
  public long Id { get; set; }
  public long QuizId { get; set; }
  public string? QuestionText { get; set; }

  // Navigation properties
  public Quiz Quiz { get; set; } = null!;
  public List<Option> Options { get; set; } = [];
}