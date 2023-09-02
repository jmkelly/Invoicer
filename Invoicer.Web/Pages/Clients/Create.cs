using Invoicer.Web.Pages.Clients.Models;
using Mapster;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Invoicer.Web.Pages.Clients
{
    public class Create : PageModel
    {
        private readonly DataContext context;

        public Create(DataContext context)
        {
            this.context = context;
        }

        [BindProperty]
        public ClientAddModel Model { get; set; }

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
