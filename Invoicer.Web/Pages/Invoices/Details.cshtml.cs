using Mapster;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Invoicer.Web.Pages.Invoices
{
	public class InvoiceDetailsModel
	{
		public required string Id { get; set; }
		public required string InvoiceCode { get; set; }
		public required ClientIndexModel Client { get; set; }
		public required DateTime CreatedAt { get; set; }
		public required InvoiceStatus InvoiceStatus { get; set; }
		public required decimal Total { get; set; }
		public required List<Entities.Hours> Hours { get; set; }
		public required MyAccount.MyAccount Account { get; set; }
	}

	public class DetailsModel : PageModel
	{
		private readonly SqliteContext context;

		public DetailsModel(SqliteContext context)
		{
			this.context = context;
		}

		public required InvoiceDetailsModel Invoice { get; set; }


		public async Task<IActionResult> OnGet(Guid id)
		{
			var entity = await context.Invoices
							.Include(i => i.Client)
							.Include(i => i.Hours)
							.Include(i => i.Account)
							.FirstOrDefaultAsync(i => i.Id == id);

			if (entity == null)
			{
				return NotFound();
			}

			Invoice = new InvoiceDetailsModel
			{
				Id = entity.Id.ToString(),
				InvoiceCode = entity.InvoiceCode,
				Client = entity.Client.Adapt<ClientIndexModel>(),
				Total = entity.Total(),
				InvoiceStatus = entity.InvoiceStatus,
				CreatedAt = entity.CreatedAt,
				Hours = entity.Hours,
				Account = entity.Account
			};
			return Page();
		}
	}
}
