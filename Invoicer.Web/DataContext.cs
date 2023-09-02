namespace Invoicer.Web;
using Invoicer.Web.Pages.Creditors.Models;
using Microsoft.EntityFrameworkCore;

public class DataContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public DataContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // connect to postgres with connection string from app settings
        options.UseNpgsql(Configuration.GetConnectionString("Default"));
    }

    public DbSet<Creditor> Creditors { get; set; }
}