using Mapster;
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
		public async Task OnGet()
		{
			var clients = await context.Clients.ToListAsync();
			Clients = clients.Select(c => ClientIndexModel.Create(c)).ToList();
		}
	}
}
