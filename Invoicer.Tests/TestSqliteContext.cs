using Invoicer.Web;
using Microsoft.EntityFrameworkCore;

namespace Invoicer.Tests;

public class TestSqliteContext : SqliteContext
{
    private readonly string _databaseName;

    public TestSqliteContext(string databaseName) : base(new Microsoft.Extensions.Configuration.ConfigurationBuilder().Build())
    {
        _databaseName = databaseName;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseInMemoryDatabase(_databaseName);
    }
} 