using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Invoicer.Web.Exceptions;

namespace Invoicer.Web.Pages.Invoices
{
	public class DeleteModel : PageModel
	{
		private readonly IInvoiceRepository _invoiceRepository;
		private readonly ILogger<DeleteModel> _logger;

		public DeleteModel(IInvoiceRepository invoiceRepository, ILogger<DeleteModel> logger)
		{
			_invoiceRepository = invoiceRepository ?? throw new ArgumentNullException(nameof(invoiceRepository));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		[BindProperty]
		public required InvoiceDeleteModel Model { get; set; }

		public async Task<IActionResult> OnGetAsync(Guid id)
		{
			try
			{
				var invoice = await _invoiceRepository.GetByIdAsync(id);
				if (invoice == null)
				{
					_logger.LogWarning("Invoice with ID {InvoiceId} not found", id);
					return NotFound();
				}

				Model = invoice.Adapt<InvoiceDeleteModel>();
				_logger.LogInformation("Retrieved invoice {InvoiceId} for deletion", id);

				return Page();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error retrieving invoice {InvoiceId} for deletion", id);
				ModelState.AddModelError("", "An error occurred while retrieving the invoice. Please try again.");
				return Page();
			}
		}

		public async Task<IActionResult> OnPostAsync(Guid id)
		{
			try
			{
				if (!await _invoiceRepository.ExistsAsync(id))
				{
					_logger.LogWarning("Invoice with ID {InvoiceId} not found for deletion", id);
					return NotFound();
				}

				await _invoiceRepository.DeleteAsync(id);
				_logger.LogInformation("Successfully deleted invoice {InvoiceId}", id);

				return RedirectToPage("Index");
			}
			catch (InvoiceNotDeletableException ex)
			{
				_logger.LogWarning(ex, "Attempted to delete non-deletable invoice {InvoiceId}", id);
				ModelState.AddModelError("", ex.Message);
				return await OnGetAsync(id);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error deleting invoice {InvoiceId}", id);
				ModelState.AddModelError("", "An error occurred while deleting the invoice. Please try again.");
				return await OnGetAsync(id);
			}
		}
	}

	public class InvoiceDeleteModel
	{
		public Guid Id { get; set; }
		public required string InvoiceCode { get; set; }
		public required string ClientName { get; set; }
		public required DateTime CreatedAt { get; set; }
		public required InvoiceStatus InvoiceStatus { get; set; }
		public decimal Total { get; set; }
	}
}
