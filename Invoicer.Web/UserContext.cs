namespace Invoicer.Web;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class UserContext : IdentityDbContext
{
	protected readonly IConfiguration Configuration;

	public UserContext(IConfiguration configuration)
	{
		Configuration = configuration;
	}

	protected override void OnConfiguring(DbContextOptionsBuilder options)
	{
		// connect to postgres with connection string from app settings
		options.UseNpgsql(Configuration.GetConnectionString("Default"));
	}

}
