namespace Invoicer.Web;

using Invoicer.Web.Pages.Clients.Models;
using Invoicer.Web.Pages.Invoices;
using Invoicer.Web.Pages.MyAccount;
using Invoicer.Web.Pages.Settings;
using Microsoft.EntityFrameworkCore;

public class SqliteContext : DbContext
{

	public SqliteContext(IConfiguration configuration)
	{
		var folder = Environment.SpecialFolder.LocalApplicationData;
		var path = Environment.GetFolderPath(folder);
		DbPath = Path.Join(path, "invoicer.db");
	}

	public string DbPath { get; }

	protected override void OnConfiguring(DbContextOptionsBuilder options)
	{
		options.UseSqlite($"Data Source={DbPath}");
	}

	public DbSet<Client> Clients { get; set; }
	public DbSet<MyAccount> MyAccounts { get; set; }
	public DbSet<Setting> Settings { get; set; }
	public DbSet<Entities.Hours> Hours { get; set; }
	public DbSet<Invoice> Invoices { get; set; }

}

