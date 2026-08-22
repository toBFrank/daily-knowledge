namespace DailyKnowledge.Api.Models;

public class Quiz
{
  public long Id { get; set; }
  public long ArticleId { get; set; }

  // Navigation properties
  public Article Article { get; set; } = null!;
  public List<Question> Questions { get; set; } = [];
}