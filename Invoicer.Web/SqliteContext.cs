namespace Invoicer.Web;

using Invoicer.Web.Pages.Clients.Models;
using Invoicer.Web.Pages.Invoices;
using Invoicer.Web.Pages.MyAccount;
using Invoicer.Web.Pages.Settings;
using Microsoft.EntityFrameworkCore;

public class SqliteContext : DbContext
{
	public SqliteContext(DbContextOptions<SqliteContext> options) : base(options)
	{
	}

	public DbSet<Client> Clients { get; set; }
	public DbSet<MyAccount> MyAccounts { get; set; }
	public DbSet<Setting> Settings { get; set; }
	public DbSet<Entities.Hours> Hours { get; set; }
	public DbSet<Invoice> Invoices { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		// Configure Invoice entity
		modelBuilder.Entity<Invoice>(entity =>
		{
			entity.HasKey(e => e.Id);
			entity.Property(e => e.InvoiceCode).IsRequired().HasMaxLength(50);
			entity.Property(e => e.CreatedAt).IsRequired();
			entity.Property(e => e.InvoiceDate).IsRequired();
			entity.Property(e => e.UpdatedAt).IsRequired();
			entity.Property(e => e.InvoiceStatus).IsRequired();
			
			entity.HasOne(e => e.Client)
				.WithMany()
				.HasForeignKey("ClientId")
				.OnDelete(DeleteBehavior.Restrict);
				
			entity.HasOne(e => e.Account)
				.WithMany()
				.HasForeignKey("AccountId")
				.OnDelete(DeleteBehavior.Restrict);
		});

		// Configure Hours entity
		modelBuilder.Entity<Entities.Hours>(entity =>
		{
			entity.HasKey(e => e.Id);
			entity.Property(e => e.Date).IsRequired();
			entity.Property(e => e.NumberOfHours).HasPrecision(5, 2);
			entity.Property(e => e.Description).IsRequired().HasMaxLength(500);
			entity.Property(e => e.Rate).HasPrecision(10, 2);
			entity.Property(e => e.RateUnits).IsRequired();
			entity.Property(e => e.ClientId).IsRequired();
			entity.Property(e => e.DateRecorded).IsRequired();
			
			entity.HasOne(e => e.Client)
				.WithMany()
				.HasForeignKey(e => e.ClientId)
				.OnDelete(DeleteBehavior.Restrict);
		});

		// Configure Client entity
		modelBuilder.Entity<Client>(entity =>
		{
			entity.HasKey(e => e.Id);
			entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
			entity.Property(e => e.CompanyName).IsRequired().HasMaxLength(100);
		});
	}
}

