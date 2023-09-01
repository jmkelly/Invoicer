using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Invoicer.Web.Pages
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

            return RedirectToPage("/Creditors");
        }

    }


    public class CreditorAddModel
    {

        public string? CompanyName { get; set; }
        public required string Name { get; set; }
        public string? BSB { get; set; }
        public string? AccountNo { get; set; }
        public string? StreetNumber { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Postcode { get; set; }
    }
}
