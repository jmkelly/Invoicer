using Invoicer.Web;
using Microsoft.EntityFrameworkCore;

namespace Invoicer.Tests;

public class TestSqliteContext : SqliteContext
{
    public TestSqliteContext(DbContextOptions<SqliteContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Don't call base.OnConfiguring for in-memory testing
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseInMemoryDatabase("TestDatabase");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Add any test-specific configurations here if needed
    }
} 