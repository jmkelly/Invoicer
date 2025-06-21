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
		private readonly IInvoiceRepository _invoiceRepository;
		private readonly ILogger<IndexViewModel> _logger;

		public IndexViewModel(IInvoiceRepository invoiceRepository, ILogger<IndexViewModel> logger)
		{
			_invoiceRepository = invoiceRepository ?? throw new ArgumentNullException(nameof(invoiceRepository));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			Invoices = new List<InvoiceIndexModel>();
		}

		public List<InvoiceIndexModel> Invoices { get; set; }

		public async Task<IActionResult> OnGetAsync(string? search, int? pageNum, int? pageSize)
		{
			try
			{
				SetPaging(search, pageNum, pageSize);

				var entities = await _invoiceRepository.SearchAsync(Search, Skip, PageSize);

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

				_logger.LogInformation("Retrieved {Count} invoices for search term '{SearchTerm}'", Invoices.Count, Search);

				return Request.IsHtmx()
					? Partial("_InvoiceRowsPartial", this)
					: Page();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error retrieving invoices for search term '{SearchTerm}'", Search);
				ModelState.AddModelError("", "An error occurred while retrieving invoices. Please try again.");
				return Page();
			}
		}
	}
}
