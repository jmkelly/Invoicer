namespace Invoicer.Web;
using Invoicer.Web.Pages.Clients.Models;
using Invoicer.Web.Pages.MyAccount;
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

    public DbSet<Client> Clients { get; set; }
    public DbSet<MyAccount> MyAccounts { get; set; }
}