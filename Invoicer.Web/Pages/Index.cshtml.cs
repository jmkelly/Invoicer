using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Invoicer.Web.Pages.Hours;
using Invoicer.Web.Pages.Invoices;
using Microsoft.AspNetCore.Mvc;
using Invoicer.Web.Extensions;
using Mapster;
using MassTransit;


namespace Invoicer.Web.Pages;

public class IndexModel : PageModel
{
	private readonly ILogger<IndexModel> logger;
	private readonly SqliteContext context;
	[BindProperty]
	public required IndexViewModel IndexViewModel { get; set; }

	public IndexModel(ILogger<IndexModel> logger, SqliteContext context)
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
			.Include(c => c.Hours)
			.ToListAsync();

		var workItems = (await context.Hours
			.Include(c => c.Client)
			.Include(c => c.Invoice)
			.ToListAsync())
			.Where(w => !w.HasInvoice()).ToList();


		var clients = await context.Clients.ToListAsync();

		var wi = new CreateHoursModel
		{
			Date = DateOnly.FromDateTime(DateTime.Now),
			NumberOfHours = 0,
			Description = string.Empty,
			Rate = 0,
			ClientId = default,
			Clients = clients,
			RateUnits = RateUnits.PerHour,
		};


		var outStandingHours = await context.Hours
			.Include(c => c.Client)
			.Where(c => c.Invoice == null).ToListAsync();

		var OutStandingHours = outStandingHours.Select(o => new InvoiceHoursModel
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
			OutStandingHours = OutStandingHours,
			SelectedHours = []
		};

		var model = new IndexViewModel
		{
			Invoices = invoices,
			OutstandingHours = workItems,
			NewHours = wi,
			NewInvoice = inv
		};


		IndexViewModel = model;
	}

	public async Task<ActionResult> OnPostAddHours()
	{
		logger.LogInformation(IndexViewModel.ToJson());
		var hours = IndexViewModel.NewHours;

		var wi = hours.Adapt<Entities.Hours>();
		wi.Id = NewId.NextSequentialGuid();
		context.Hours.Add(wi);
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

		List<Guid> ids = vm.OutStandingHours.Where(c => c.IsSelected).Select(c => c.Id).ToList();
		List<Entities.Hours> wi = await context.Hours
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
				foreach (Entities.Hours? item in wi.Where(c => c.Client == client))
				{
					invoice.AddHours(item);
					context.Invoices.Add(invoice);
				}
			}
		}

		await context.SaveChangesAsync();
		return RedirectToPage("Index");

	}
}
