using Invoicer.Web.Pages.Clients.Models;
using Mapster;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Invoicer.Web.Pages.Clients
{
	public class Create(DataContext context) : PageModel
	{
		private readonly DataContext context = context;

		[BindProperty]
		public required ClientAddModel Model { get; set; }

		public void OnGet()
		{
		}

		public async Task<IActionResult> OnPost()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}
			var Client = Model.Adapt<Client>();
			//create a sequential uuid
			Client.Id = NewId.NextSequentialGuid();
			context.Add(Client);
			await context.SaveChangesAsync();

			return RedirectToPage("Index");
		}

	}
}
