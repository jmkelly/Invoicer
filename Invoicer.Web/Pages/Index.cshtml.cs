using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Invoicer.Web.Pages.Invoices;


namespace Invoicer.Web.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly DataContext context;
	public InvoicesIndexModel InvoicesIndexModel {get;set;}

    public IndexModel(ILogger<IndexModel> logger, DataContext context)
    {
        _logger = logger;
        this.context = context;
    }

    public async Task OnGet()
    {
		var invoices = await context
			.Invoices
			.Include(c => c.Client)
			.Include(c => c.Account)
			.Include(c => c.WorkItems)
			.ToListAsync();

		var workItems = (await context.WorkItems
			.Include(c => c.Client)
			.Include(c => c.Invoice)
			.ToListAsync())
			.Where(w => !w.HasInvoice()).ToList();


		var model = new InvoicesIndexModel();
		foreach (var invoice in invoices)
		{
			model.Invoices.Add(invoice);
		}

		foreach (var wi in workItems)
		{
			model.OutstandingWorkItems.Add(wi);
		}

		InvoicesIndexModel = model;
    }
}
