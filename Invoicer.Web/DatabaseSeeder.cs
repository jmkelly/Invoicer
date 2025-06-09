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

			if (!context.WorkItems.Any())
			{
				var workItems = SeedDataGenerator.GenerateWorkItems(1000, clients);
				context.WorkItems.AddRange(workItems);
			}


			await context.SaveChangesAsync();
		}
	}
}
