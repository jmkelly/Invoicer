using Mapster;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Invoicer.Web.Pages.Invoices
{
	public class ClientIndexModel
	{
		public required string Name { get; set; }
		public required string CompanyName { get; set; }

	}
	public class InvoiceIndexModel
	{
		public required string Id { get; set; }
		public required string InvoiceCode { get; set; }
		public required ClientIndexModel Client { get; set; }
		public required DateTime CreatedAt { get; set; }
		public required DateTime InvoiceDate { get; set; }
		public required InvoiceStatus InvoiceStatus { get; set; }
		public required decimal Total { get; set; }
		public string? AccountLabel { get; set; }
	}

	public class IndexViewModel : PageModel
	{
		private readonly DataContext context;

		public IndexViewModel(DataContext context)
		{
			this.context = context;
			Invoices = new List<InvoiceIndexModel>();
		}

		public List<InvoiceIndexModel> Invoices { get; set; }
		public async Task OnGet()
		{
			var entities = await context.Invoices
							.Include(i => i.Client)
							.Include(i => i.Hours)
							.Include(i => i.Account)
							.ToListAsync();

			entities.OrderByDescending(i => i.CreatedAt);

			Invoices = entities.Select(i =>
								new InvoiceIndexModel
								{
									Id = i.Id.ToString(),
									InvoiceCode = i.InvoiceCode,
									Client = i.Client.Adapt<ClientIndexModel>(),
									Total = i.Total(),
									InvoiceStatus = i.InvoiceStatus,
									CreatedAt = i.CreatedAt,
									InvoiceDate = i.InvoiceDate,
									AccountLabel = i.Account.Label()
								})
							.ToList();


		}
	}
}
