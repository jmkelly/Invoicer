using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Invoicer.Web.Pages.WorkItems;
using Invoicer.Web.Pages.Invoices;
using Microsoft.AspNetCore.Mvc;
using Invoicer.Web.Extensions;
using Mapster;
using MassTransit;


namespace Invoicer.Web.Pages;

public class IndexModel : PageModel
{
	private readonly ILogger<IndexModel> logger;
	private readonly DataContext context;
	[BindProperty]
	public required IndexViewModel IndexViewModel { get; set; }

	public IndexModel(ILogger<IndexModel> logger, DataContext context)
	{
		this.logger = logger;
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


		var outStandingWorkItems = await context.WorkItems
			.Include(c => c.Client)
			.Where(c => c.Invoice == null).ToListAsync();

		var OutStandingWorkItems = outStandingWorkItems.Select(o => new WorkItemInvoiceModel
		{
			Id = o.Id,
			Description = o.Description,
			Total = o.Total(),
			ClientName = o.Client?.Name ?? "Unknown",
			ClientId = o.Client?.Id,
			IsSelected = true //set selected by default
		}).ToList();

		var inv = new InvoiceAddModel
		{
			ClientId = default,
			Accounts = (await context.MyAccounts.ToListAsync()),
			OutStandingWorkItems = OutStandingWorkItems,
			SelectedWorkItems = []
		};

		var model = new IndexViewModel
		{
			Invoices = invoices,
			OutstandingWorkItems = workItems,
			NewWorkItem = wi,
			NewInvoice = inv
		};


		IndexViewModel = model;
	}

	public async Task<ActionResult> OnPostAddHours()
	{
		logger.LogInformation(IndexViewModel.ToJson());
		var hours = IndexViewModel.NewWorkItem;

		var wi = hours.Adapt<WorkItem>();
		wi.Id = NewId.NextSequentialGuid();
		context.WorkItems.Add(wi);
		await context.SaveChangesAsync();
		return RedirectToPage("Index");
	}
}
