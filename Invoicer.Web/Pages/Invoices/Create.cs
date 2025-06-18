using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Invoicer.Web.Extensions;
using Invoicer.Web.Pages.Hours;

namespace Invoicer.Web.Pages.Invoices
{
	public class Create : PageModel
	{
		private readonly SqliteContext context;
		private readonly ILogger<Create> logger;

		public Create(SqliteContext context, ILogger<Create> logger)
		{
			this.context = context;
			this.logger = logger;
		}

		public required InvoiceAddModel Model { get; set; }

		[BindProperty]

		public required List<InvoiceHoursModel> OutStandingHours { get; set; }

		public required List<SelectListItem> Accounts { get; set; }

		[BindProperty]
		public Guid SelectedAccountId { get; set; }

		public async Task<IActionResult> OnGet()
		{
			var outStandingHours = await context.Hours
			.Include(c => c.Client)
			.Where(c => c.Invoice == null).ToListAsync();

			logger.LogInformation("outstanding work items {OutStandingHours}", outStandingHours.Count);

			OutStandingHours = outStandingHours.Select(o => new InvoiceHoursModel
			{
				Id = o.Id,
				Description = o.Description,
				Total = o.Total(),
				ClientName = o.Client?.Name ?? "Unknown",
				ClientId = o.Client?.Id,
				IsSelected = true //set selected by default
			}).ToList();

			var accounts = await context.MyAccounts.ToListAsync();

			Accounts = accounts.Select(a => new SelectListItem { Text = a.Label(), Value = a.Id.ToString() }).ToList();
			logger.LogInformation("got {count} accounts", Accounts.Count);

			return Page();
		}

		public async Task<IActionResult> OnPost()
		{
			//log stuff
			logger.LogInformation("outstanding work items {@outstandingHours}", OutStandingHours);
			logger.LogInformation("post data {@form}", this.Request.Form);
			var hours = OutStandingHours.Where(w => w.IsSelected).Select(c => new HoursAddModel { Id = c.Id }).ToList();
			logger.LogInformation("getting into post");
			if (!ModelState.IsValid)
			{
				var message = string.Join(" | ", ModelState.Values
					.SelectMany(v => v.Errors)
					.Select(e => e.ErrorMessage));
				logger.LogInformation("message {message}", message);
				TempData.AddMessage(message);
				return await OnGet();
			}

			if (hours == null || !hours.Any())
			{

				logger.LogWarning("no workitem selected");
				TempData.AddMessage("No items selected");
				return RedirectToPage("Create");
			}

			logger.LogInformation("works items selected {@hours}", hours.ToJson());

			//brute force this, refactor to linq later for some reason I couldn't get the commented very below to work
			//var selectedHours = await context.Hours.Where(c => hours.All(w => w.Id == c.Id)).ToListAsync();
			//
			var allhours = await context.Hours
			.Include(c => c.Client)
			.Where(c => c.Invoice == null).ToListAsync();

			logger.LogInformation("got {count} workitems", allhours.Count());

			List<Entities.Hours> selectedHours = (from i in hours
												  let wi = allhours.FirstOrDefault(w => w.Id == i.Id)
												  where wi != null
												  select wi).ToList();
			logger.LogInformation("got {count} selected work items", selectedHours.Count());
			//check that there are not muliple clients because an invoice can only apply to one client
			var clientIds = selectedHours.Select(c => c.ClientId).Distinct().ToList();
			if (clientIds.Count() > 1)
			{
				logger.LogWarning("muliple client ids selected for invoice.  ClientIds of {clientIds}", string.Join(',', clientIds));
				//FIX: These messages are not showing in the razor page
				TempData.AddMessage($"Multiple clients selected.  An invoice can only be created for one client at a time.");
				return RedirectToPage("Create");
			}

			var clientId = clientIds.First();

			//get the client
			var client = await context.Clients.FirstOrDefaultAsync(c => c.Id == clientId);
			if (client == null)
			{
				logger.LogWarning("No client found iwth id {clientId}", clientId);
				TempData.AddMessage($"No client found with id {clientId}");
				return RedirectToPage("Create");
			}


			var account = await context.MyAccounts.FirstOrDefaultAsync(a => a.Id == SelectedAccountId);
			if (account == null)
			{
				logger.LogWarning("account with id of {accountId} no longer exists", SelectedAccountId);
				return RedirectToPage("Create");
			}

			var createdAt = DateTime.UtcNow;
			var invoice = new Invoice
			{
				//create a sequential uuid
				Client = client,
				Id = NewId.NextSequentialGuid(),
				CreatedAt = createdAt,
				InvoiceDate = createdAt,
				InvoiceStatus = InvoiceStatus.Created,
				InvoiceCode = Invoice.CreateInvoiceCode(
					client.Name,
					createdAt
				),
				Account = account
			};

			foreach (var wi in selectedHours)
			{
				//add work to invoice
				//get the work item from the db
				var result = invoice.AddHours(wi);
				if (!result.IsSuccess)
				{
					TempData.Add("error", result.Error);
					return RedirectToPage("Create");
				}
			}
			context.Add(invoice);
			await context.SaveChangesAsync();
			return RedirectToPage("Index");
		}
	}
}
