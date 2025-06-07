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

	public async Task<ActionResult> OnPostCreateInvoice()
	{
		logger.LogInformation(IndexViewModel.ToJson());
		InvoiceAddModel? vm = IndexViewModel.NewInvoice;
		if (vm is null)
		{
			logger.LogWarning("vm is null");
			return RedirectToPage("Index");
		}

		List<Guid> ids = vm.OutStandingWorkItems.Where(c => c.IsSelected).Select(c => c.Id).ToList();
		List<WorkItem> wi = await context.WorkItems
			.Include(c => c.Client)
			.Where(c => ids.Any(h => h == c.Id))
			.ToListAsync();
		MyAccount.MyAccount? account = await context.MyAccounts.FirstOrDefaultAsync(c => c.Id == vm.SelectedAccountId);

		if (account is null)
		{
			logger.LogWarning("account {id} not found", vm.SelectedAccountId);
			return RedirectToPage("Index");
		}

		//select the distinct clients
		List<Clients.Models.Client?>? clients = wi.Select(c => c.Client).Distinct().ToList();
		if (clients is null)
		{
			logger.LogWarning("clients is null");
			return RedirectToPage("Index");
		}

		foreach (Clients.Models.Client? client in clients)
		{
			if (client is not null)
			{
				Invoice invoice = new Invoice
				{
					Client = client,
					CreatedAt = DateTime.UtcNow,
					InvoiceDate = DateTime.UtcNow,
					UpddatedAt = DateTime.UtcNow,
					InvoiceCode = Invoice.CreateInvoiceCode(client.Name, DateTime.UtcNow),
					InvoiceStatus = InvoiceStatus.Created,
					Account = account,
				};
				foreach (WorkItem? item in wi.Where(c => c.Client == client))
				{
					invoice.AddWorkItem(item);
					context.Invoices.Add(invoice);
				}
			}
		}

		await context.SaveChangesAsync();
		return RedirectToPage("Index");

	}
}
