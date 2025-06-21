using Invoicer.Web.Pages.Clients.Models;
using Invoicer.Web.Pages.Clients;
using Microsoft.EntityFrameworkCore;

namespace Invoicer.Web
{
	public class DatabaseSeeder(SqliteContext context)
	{
		private readonly SqliteContext context = context;

		public async Task SeedDataAsync()
		{
			List<Client> clients = [];
			if (!context.Clients.Any())
			{
				clients = await SeedDataGenerator.GenerateClientsAsync(context, 200);
				context.Clients.AddRange(clients);
			}
			else
			{
				clients = await context.Clients.ToListAsync();
			}

			// Fill missing client codes if any
			if (await context.Clients.AnyAsync(c => string.IsNullOrEmpty(c.ClientCode)))
			{
				await FillMissingClientCodesAsync();
			}

			if (!context.Hours.Any())
			{
				var hours = SeedDataGenerator.GenerateHours(1000, clients);
				context.Hours.AddRange(hours);
			}

			await context.SaveChangesAsync();

			// Fix all existing invoice codes to use ClientCode
			await FixInvoiceCodesToUseClientCodeAsync();
		}

		public async Task FillMissingClientCodesAsync()
		{
			var clientsWithoutCode = await context.Clients
				.Where(c => string.IsNullOrEmpty(c.ClientCode))
				.ToListAsync();

			foreach (var client in clientsWithoutCode)
			{
				client.ClientCode = await ClientCodeGenerator.GenerateUniqueClientCodeAsync(context, client.CompanyName, client.Name);
			}

			await context.SaveChangesAsync();
		}

		public async Task FixInvoiceCodesToUseClientCodeAsync()
		{
			var invoices = await context.Invoices
				.Include(i => i.Client)
				.ToListAsync();

			foreach (var invoice in invoices)
			{
				if (invoice.Client != null && !string.IsNullOrEmpty(invoice.Client.ClientCode))
				{
					invoice.InvoiceCode = Pages.Invoices.InvoiceCodeGenerator.CreateInvoiceCode(
						invoice.Client.ClientCode,
						invoice.CreatedAt
					);
				}
			}

			await context.SaveChangesAsync();
		}
	}
}
