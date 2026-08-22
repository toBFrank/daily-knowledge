namespace DailyKnowledge.Api.Models;

public class Article
{
  public long Id { get; set; }
  public DateTime FeaturedDate { get; set; }
  public string? Title { get; set; }
  public string? FullText { get; set; }
  public string? source_url { get; set; }
  public DateTime CreatedAt { get; set; }

  // Navigation properties
  public Quiz? Quiz { get; set; }

}