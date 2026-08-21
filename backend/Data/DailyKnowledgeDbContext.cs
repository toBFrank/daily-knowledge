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
    // public DbSet<TodoItem> TodoItems { get; set; } = null!;
}