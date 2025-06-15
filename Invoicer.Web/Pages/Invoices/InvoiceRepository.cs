using Microsoft.EntityFrameworkCore;
using Invoicer.Web.Pages.Hours;

namespace Invoicer.Web.Pages.Invoices
{
	public class InvoiceRepository : IInvoiceRepository
	{
		private readonly DataContext context;
		private readonly ILogger<InvoiceRepository> logger;

		public InvoiceRepository(DataContext context, ILogger<InvoiceRepository> logger)
		{
			this.context = context;
			this.logger = logger;
		}

		public async Task<Invoice> Get(Guid id)
		{
			return await context.Invoices
							.Include(wi => wi.Hours)
							.FirstAsync(i => i.Id == id);
		}

		public async Task Remove(Guid id)
		{
			//get the invoice and remove all the work items prior to removing
			var invoice = await Get(id);
			if (invoice == null)
			{
				logger.LogWarning("invoice id {id} does not exists", id);
				return;
			}
			invoice.Hours = new List<Entities.Hours>();
			context.Remove(invoice);
		}
		public async Task SaveChangesAsync()
		{
			await context.SaveChangesAsync();
		}
	}
}

