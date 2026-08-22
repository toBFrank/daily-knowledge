using DailyKnowledge.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DailyKnowledge.Api.Data;

public class DailyKnowledgeDbContext : DbContext
{
    public DailyKnowledgeDbContext(DbContextOptions<DailyKnowledgeDbContext> options)
        : base(options)
    {
    }

    public DbSet<Article> Articles { get; set; } = null!;
    public DbSet<Quiz> Quizzes { get; set; } = null!;
    public DbSet<Question> Questions { get; set; } = null!;
    public DbSet<Option> Options { get; set; } = null!;
    public DbSet<Payment> Payments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Article HAS 1 Quiz
        modelBuilder.Entity<Article>()
            .HasOne(a => a.Quiz)
            .WithOne(q => q.Article)
            .HasForeignKey<Quiz>(q => q.ArticleId)
            .IsRequired(false);
        // Quiz HAS MANY Questions
        modelBuilder.Entity<Quiz>()
            .HasMany(q => q.Questions)
            .WithOne(q => q.Quiz)
            .HasForeignKey(q => q.QuizId)
            .OnDelete(DeleteBehavior.Cascade);
        // Question HAS MANY Options
        modelBuilder.Entity<Question>()
            .HasMany(q => q.Options)
            .WithOne(o => o.Question)
            .HasForeignKey(o => o.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}