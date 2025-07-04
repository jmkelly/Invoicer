using Invoicer.Web.Pages.Clients.Models;
using Mapster;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Invoicer.Web.Pages.Clients
{
	public class Create(SqliteContext context) : PageModel
	{
		private readonly SqliteContext context = context;

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
			
			// Generate unique client code first
			var clientCode = await ClientCodeGenerator.GenerateUniqueClientCodeAsync(context, Model.CompanyName, Model.Name);
			
			var Client = Model.Adapt<Client>();
			//create a sequential uuid
			Client.Id = NewId.NextSequentialGuid();
			
			// Set the generated client code
			Client.ClientCode = clientCode;
			
			context.Add(Client);
			await context.SaveChangesAsync();

			return RedirectToPage("Index");
		}

	}
}
