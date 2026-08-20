namespace DailyKnowledge.Api.Models;

public class Question
{
  public long Id { get; set; }
  public long QuizId { get; set; }
  public string? QuestionText { get; set; }
}