using Mapster;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Invoicer.Web.Pages.Clients
{
	public class DetailsModel : PageModel
	{
		private readonly SqliteContext context;

		public DetailsModel(SqliteContext context)
		{
			this.context = context;
		}

		public required ClientIndexModel Client { get; set; }
		public async Task OnGet(Guid id)
		{
			var client = await context.Clients.FirstAsync(c => c.Id == id);
			Client = client.Adapt<ClientIndexModel>();
		}
	}
}
