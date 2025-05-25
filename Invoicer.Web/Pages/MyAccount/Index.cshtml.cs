using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Invoicer.Web.Pages.MyAccount
{
	public class Index : PageModel
	{
		private readonly DataContext context;

		public Index(DataContext context)
		{
			this.context = context;
		}

		public required List<MyAccountIndexModel> MyAccounts { get; set; }

		public async Task OnGet()
		{
			var myaccounts = await context.MyAccounts.ToListAsync();
			MyAccounts = myaccounts.Adapt<List<MyAccountIndexModel>>();
		}
	}
}
