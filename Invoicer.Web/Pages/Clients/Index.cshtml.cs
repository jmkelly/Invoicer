using Mapster;
using Microsoft.AspNetCore.Mvc;
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

        public List<ClientIndexModel> Clients {get;set;}
        public async Task OnGet()
        {
            var clients =  await context.Clients.ToListAsync();
            Clients = clients.Adapt<List<ClientIndexModel>>();
        }
    }
}
