using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Htmx;

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

	public class IndexViewModel : PagedModel
	{
		private readonly SqliteContext context;
		private readonly ILogger<IndexViewModel> logger;

		public IndexViewModel(SqliteContext context, ILogger<IndexViewModel> logger)
		{
			this.context = context;
			this.logger = logger;
			Invoices = new List<InvoiceIndexModel>();
		}

		public List<InvoiceIndexModel> Invoices { get; set; }

		public async Task<IActionResult> OnGetAsync(string? search, int? pageNum, int? pageSize)
		{
			SetPaging(search, pageNum, pageSize);

			var query = context.Invoices
				.Include(i => i.Client)
				.Include(i => i.Hours)
				.Include(i => i.Account)
				.AsQueryable();

			if (!string.IsNullOrWhiteSpace(Search))
			{
				logger.LogInformation("searching for {Search}", Search);
				var searchLower = Search.ToLower();
					query = query.Where(i =>
						i.InvoiceCode.ToLower().Contains(searchLower) ||
						i.Client.Name.ToLower().Contains(searchLower) 
					);
			}

			var entities = await query
				.OrderByDescending(i => i.CreatedAt)
				.Skip(Skip)
				.Take(PageSize)
				.ToListAsync();

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

			return Request.IsHtmx()
				? Partial("_InvoiceRowsPartial", this)
				: Page();
		}
	}
}
