using Invoicer.Web.Pages.Clients.Models;
using Microsoft.EntityFrameworkCore;

namespace Invoicer.Web
{
	public class DatabaseSeeder(DataContext context)
	{
		private readonly DataContext context = context;

		public async Task SeedDataAsync()
		{
			List<Client> clients = [];
			if (!context.Clients.Any())
			{
				clients = SeedDataGenerator.GenerateClients(200);
				context.Clients.AddRange(clients);
			}
			else
			{
				clients = await context.Clients.ToListAsync();
			}

			if (!context.Hours.Any())
			{
				var hours = SeedDataGenerator.GenerateHours(1000, clients);
				context.Hours.AddRange(hours);
			}


			await context.SaveChangesAsync();
		}
	}
}
