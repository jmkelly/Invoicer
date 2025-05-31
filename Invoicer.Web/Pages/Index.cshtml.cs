using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Invoicer.Web.Pages.WorkItems;


namespace Invoicer.Web.Pages;

public class IndexModel : PageModel
{
	private readonly ILogger<IndexModel> _logger;
	private readonly DataContext context;
	public IndexViewModel IndexViewModel { get; set; }

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


		var clients = await context.Clients.ToListAsync();

		var wi = new CreateWorkItemModel
		{
			Date = DateOnly.FromDateTime(DateTime.Now),
			Hours = 0,
			Description = string.Empty,
			Rate = 0,
			ClientId = default,
			Clients = clients,
			RateUnits = RateUnits.PerHour,
		};

		var model = new IndexViewModel
		{
			Invoices = invoices,
			OutstandingWorkItems = workItems,
			NewWorkItem = wi
		};


		IndexViewModel = model;
	}
}
