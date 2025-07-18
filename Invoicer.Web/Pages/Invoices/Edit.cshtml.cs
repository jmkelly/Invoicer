using Mapster;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Invoicer.Web.Extensions;

namespace Invoicer.Web.Pages.Invoices
{
	public class EditModel : PageModel
	{
		private readonly SqliteContext context;
		private readonly ILogger<EditModel> logger;
		private readonly IInvoiceRepository invoiceRepository;

		public EditModel(IInvoiceRepository invoiceRepository, SqliteContext context, ILogger<EditModel> logger)
		{
			this.invoiceRepository = invoiceRepository;
			this.context = context;
			this.logger = logger;
		}

		[BindProperty]
		public InvoiceEditModel Invoice { get; set; }

		public async Task<IActionResult> OnPostRemoveHourAsync(Guid id, Guid hourId)
		{
			var invoice = await context.Invoices
							.Include(i => i.Client)
							.Include(i => i.Hours)
							.FirstOrDefaultAsync(i => i.Id == id);

			if (invoice == null)
			{
				return NotFound();
			}

			// Only allow removing hours if invoice is in Created status
			if (invoice.InvoiceStatus != InvoiceStatus.Created)
			{
				TempData.AddErrorMessage("Cannot remove hours from an invoice that is not in Created status.");
				return RedirectToPage(new { id });
			}

			var hourToRemove = invoice.Hours.FirstOrDefault(h => h.Id == hourId);
			if (hourToRemove == null)
			{
				return NotFound();
			}

			invoice.Hours.Remove(hourToRemove);
			await context.SaveChangesAsync();

			TempData.AddSuccessMessage("Hour entry removed successfully.");
			return RedirectToPage(new { id });
		}

		public async Task<IActionResult> OnPost(Guid id)
		{

			logger.LogInformation(Invoice.ToJson());

			//remove all the work items
			var invoice = await context.Invoices
							.Include(i => i.Client)
							.Include(i => i.Hours)
							.FirstOrDefaultAsync(i => i.Id == id);

			if (invoice == null)
			{
				return NotFound();
			}

			// Only allow editing if invoice is not in Sent status
			if (invoice.InvoiceStatus == InvoiceStatus.Sent)
			{
				TempData.AddErrorMessage("Cannot edit an invoice that has been sent.");
				return RedirectToPage(new { id });
			}

			invoice.InvoiceStatus = Invoice.InvoiceStatus;
			invoice.InvoiceDate = Invoice.InvoiceDate;
			foreach (var wi in invoice.Hours)
			{
				wi.Description = Invoice.Hours.First(h => h.Id == wi.Id).Description;
				wi.NumberOfHours = Invoice.Hours.First(h => h.Id == wi.Id).NumberOfHours;
			}

			await context.SaveChangesAsync();

			return RedirectToPage("./Index");
		}


		public async Task<IActionResult> OnGet(Guid id)
		{
			var entity = await context.Invoices
							.Include(i => i.Client)
							.Include(i => i.Hours)
							.FirstOrDefaultAsync(i => i.Id == id);

			if (entity == null)
			{
				return NotFound();
			}

			Invoice = new InvoiceEditModel
			{
				Id = entity.Id.ToString(),
				InvoiceCode = entity.InvoiceCode,
				Client = entity.Client.Adapt<ClientIndexModel>(),
				Total = entity.Total(),
				InvoiceStatus = entity.InvoiceStatus,
				Statuses = EnumHelper.ToSelectList<InvoiceStatus>(),
				CreatedAt = entity.CreatedAt,
				InvoiceDate = entity.InvoiceDate,
				Hours = [.. entity.Hours]
			};
			return Page();
		}
	}
}
