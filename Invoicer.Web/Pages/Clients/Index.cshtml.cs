using Htmx;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Invoicer.Web.Pages.Clients
{
	public class IndexModel(DataContext context, ILogger<IndexModel> logger) : PageModel
	{
		private readonly DataContext context = context;
		private readonly ILogger<IndexModel> logger = logger;

		public required List<ClientIndexModel> Clients { get; set; }
		public int PageSize { get; set; } = 20;
		public int PageNum { get; set; } = 1;
		[BindProperty(SupportsGet = true)]
		public string Search { get; set; } = string.Empty;

		public async Task<IActionResult> OnGetAsync(string? search, int? pageNum, int? pageSize)
		{
			PageSize = pageSize ?? 20;
			PageNum = pageNum ?? 1;
			var skip = (PageNum - 1) * PageSize;
			Search = search ?? string.Empty;

			var clients = context.Clients.AsQueryable();

			if (!string.IsNullOrWhiteSpace(Search))
			{
				logger.LogInformation("searching for {Search}", Search);
				//this isn't ideal.  If we keep to pg, then usen EF.Function.Ilike but i currently
				//plan to switch to sqlite so this may still be need to be kept
				clients = clients.Where(c =>
						c.Name.ToLower().Contains(Search.ToLower()) ||
						c.CompanyName!.ToLower().Contains(Search.ToLower()) ||
						c.Street!.ToLower().Contains(Search.ToLower()) ||
						c.City!.ToLower().Contains(Search.ToLower())
						);
			}

			var clientsList = await clients.OrderBy(c => c.Name).Skip(skip).Take(PageSize).ToListAsync();

			Clients = [.. clientsList.Select(ClientIndexModel.Create)];


			return Request.IsHtmx()
				? Partial("_ClientRowsPartial", this)
				: Page();

		}
	}
}
