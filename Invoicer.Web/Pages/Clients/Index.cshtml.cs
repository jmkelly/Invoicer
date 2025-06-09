using Htmx;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Invoicer.Web.Pages.Clients
{
	public class IndexModel : PageModel
	{
		private readonly DataContext context;

		public IndexModel(DataContext context)
		{
			this.context = context;
		}

		public required List<ClientIndexModel> Clients { get; set; }
		public int PageSize { get; set; } = 20;
		public int PageNum { get; set; } = 1;
		public string Search { get; set; } = string.Empty;

		public async Task<IActionResult> OnGetAsync(string search = "", int pageNum = 1, int pageSize = 20)
		{
			var skip = (pageNum - 1) * pageSize;
			PageNum = pageNum;
			PageSize = pageSize;
			Search = search ?? string.Empty;

			var clients = await context.Clients.OrderBy(c => c.Name).Skip(skip).Take(pageSize).ToListAsync();
			if (string.IsNullOrWhiteSpace(Search))
			{
				clients = [.. clients.Where(c => c.SearchString.Contains(Search, StringComparison.InvariantCultureIgnoreCase))];
			}

			var isHtmx = HttpContext.Request.IsHtmx();

			Clients = clients.Select(ClientIndexModel.Create).ToList();

			return Request.IsHtmx()
				? Partial("_ClientRowsPartial", this)
				: Page();

		}
	}
}
