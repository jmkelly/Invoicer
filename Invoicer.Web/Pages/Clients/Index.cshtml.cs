using Htmx;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Invoicer.Web.Pages.Clients
{
	public class IndexModel(SqliteContext context, ILogger<IndexModel> logger) : PagedModel
	{
		private readonly SqliteContext context = context;
		private readonly ILogger<IndexModel> logger = logger;

		public required List<ClientIndexModel> Clients { get; set; }

		public async Task<IActionResult> OnGetAsync(string? search, int? pageNum, int? pageSize)
		{
			SetPaging(search, pageNum, pageSize);

			var clients = context.Clients.AsQueryable();

			if (!string.IsNullOrWhiteSpace(Search))
			{
				logger.LogInformation("searching for {Search}", Search);
				//this isn't ideal.  If we keep to pg, then usen EF.Func
				//plan to switch to sqlite so this may still be need to be kept
				clients = clients.Where(c =>
						c.Name.ToLower().Contains(Search.ToLower()) ||
						c.CompanyName!.ToLower().Contains(Search.ToLower()) ||
						c.Street!.ToLower().Contains(Search.ToLower()) ||
						c.City!.ToLower().Contains(Search.ToLower())
						);
			}

			var clientsList = await clients.OrderBy(c => c.Name).Skip(Skip).Take(PageSize).ToListAsync();

			Clients = [.. clientsList.Select(ClientIndexModel.Create)];


			return Request.IsHtmx()
				? Partial("_ClientRowsPartial", this)
				: Page();

		}
	}
}
