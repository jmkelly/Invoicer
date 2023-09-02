using Invoicer.Web.Pages.Creditors.Models;
using Mapster;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Invoicer.Web.Pages.Creditors
{
    public class AddCreditorModel : PageModel
    {
        public AddCreditorModel()
        {
        }

        [BindProperty]
        public CreditorAddModel Model { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            var creditor = Model.Adapt<Creditor>();
            //create a sequential uuid
            creditor.Id = NewId.NextSequentialGuid();

            return RedirectToPage("/Creditors");
        }

    }
}
