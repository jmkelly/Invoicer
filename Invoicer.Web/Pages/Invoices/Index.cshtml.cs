using Mapster;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Invoicer.Web.Pages.Invoices
{
    public class ClientIndexModel 
    {
        public required string Name { get; set; }

    }
    public class InvoiceIndexModel
    {
        public required string Id {get;set;}
        public required string InvoiceCode{get;set;}
        public required ClientIndexModel Client {get;set;}
        public required DateTime CreatedAt {get;set;}
        public required InvoiceStatus InvoiceStatus {get;set;}

        public required decimal Total {get;set;}
    }
    public class IndexModel : PageModel
    {
        private readonly DataContext context;

        public IndexModel(DataContext context)
        {
            this.context = context;
        }

        public List<InvoiceIndexModel> Invoices {get;set;}
        public async Task OnGet()
        {
            var invoices = await context.Invoices.Include(i => i.Client).ToListAsync();
            var model = invoices.Adapt<List<InvoiceIndexModel>>();
            Invoices = model;
        }
    }
}
