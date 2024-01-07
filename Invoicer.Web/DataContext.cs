namespace Invoicer.Web;
using Invoicer.Web.Pages.Clients.Models;
using Invoicer.Web.Pages.Invoices;
using Invoicer.Web.Pages.MyAccount;
using Invoicer.Web.Pages.Settings;
using Invoicer.Web.Pages.WorkItems;
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
	public DbSet<Setting> Settings { get; set; }
    public DbSet<WorkItem> WorkItems { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
}
