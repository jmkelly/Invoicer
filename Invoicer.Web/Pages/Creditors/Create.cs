using Invoicer.Web.Pages.Creditors.Models;
using Mapster;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Invoicer.Web.Pages.Creditors
{
    public class Create : PageModel
    {
        private readonly DataContext context;

        public Create(DataContext context)
        {
            this.context = context;
        }

        [BindProperty]
        public CreditorAddModel Model { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            var creditor = Model.Adapt<Creditor>();
            //create a sequential uuid
            creditor.Id = NewId.NextSequentialGuid();
            context.Add(creditor);
            await context.SaveChangesAsync();


            return RedirectToPage("/Creditors");
        }

    }
}
