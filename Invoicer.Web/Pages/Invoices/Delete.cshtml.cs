using Mapster;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Invoicer.Web.Pages.Invoices
{
	public class DeleteModel : PageModel
	{
		private readonly SqliteContext context;
		private readonly IInvoiceRepository invoiceRepository;

		public DeleteModel(IInvoiceRepository invoiceRepository, SqliteContext context)
		{
			this.invoiceRepository = invoiceRepository;
			this.context = context;
		}

		public required InvoiceIndexModel Invoice { get; set; }

		public async Task<IActionResult> OnPost(Guid id)
		{
			//remove all the work items
			await invoiceRepository.Remove(id);
			await invoiceRepository.SaveChangesAsync();

			return RedirectToPage("Index");
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

			Invoice = new InvoiceIndexModel
			{
				Id = entity.Id.ToString(),
				InvoiceCode = entity.InvoiceCode,
				Client = entity.Client.Adapt<ClientIndexModel>(),
				Total = entity.Total(),
				InvoiceStatus = entity.InvoiceStatus,
				InvoiceDate = entity.InvoiceDate,
				CreatedAt = entity.CreatedAt
			};
			return Page();
		}
	}
}
