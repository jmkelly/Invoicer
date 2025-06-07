using Mapster;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Invoicer.Web.Extensions;

namespace Invoicer.Web.Pages.Invoices
{
	public class EditModel : PageModel
	{
		private readonly DataContext context;
		private readonly ILogger<EditModel> logger;
		private readonly IInvoiceRepository invoiceRepository;

		public EditModel(IInvoiceRepository invoiceRepository, DataContext context, ILogger<EditModel> logger)
		{
			this.invoiceRepository = invoiceRepository;
			this.context = context;
			this.logger = logger;
		}

		[BindProperty]
		public InvoiceEditModel Invoice { get; set; }

		public async Task<IActionResult> OnPost(Guid id)
		{

			logger.LogInformation(Invoice.ToJson());

			//remove all the work items
			var invoice = await context.Invoices
							.Include(i => i.Client)
							.Include(i => i.WorkItems)
							.FirstOrDefaultAsync(i => i.Id == id);

			if (invoice == null)
			{
				return NotFound();
			}

			invoice.InvoiceStatus = Invoice.InvoiceStatus;
			invoice.InvoiceDate = Invoice.InvoiceDate;
			foreach (var wi in invoice.WorkItems)
			{
				wi.Description = Invoice.Hours.First(h => h.Id == wi.Id).Description;
				wi.Hours = Invoice.Hours.First(h => h.Id == wi.Id).Hours;
			}

			await context.SaveChangesAsync();

			return RedirectToPage("./Index");
		}


		public async Task<IActionResult> OnGet(Guid id)
		{
			var entity = await context.Invoices
							.Include(i => i.Client)
							.Include(i => i.WorkItems)
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
				Hours = [.. entity.WorkItems]
			};
			return Page();
		}
	}
}
