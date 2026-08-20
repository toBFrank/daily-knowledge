namespace DailyKnowledge.Api.Models;

public class Payment
{
  public long Id { get; set;}
  public long StripeSessionId { get; set; }
  public long AmountCents { get; set; }
  public string? Currency { get; set; }
  public PaymentStatus Status { get; set; }

}

public enum PaymentStatus
{
  Pending = 1,
  Completed = 2,
  Failed = 3
}