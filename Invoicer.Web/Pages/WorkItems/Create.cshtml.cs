using Mapster;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Invoicer.Web.Extensions;

namespace Invoicer.Web.Pages.WorkItems;

public class Create : PageModel
{
	private readonly DataContext context;
	private readonly ILogger<Create> logger;

	public Create(DataContext context, ILogger<Create> logger)
	{
		this.context = context;
		this.logger = logger;
	}

	[BindProperty]
	public Guid ClientId { get; set; }

	[BindProperty]
	public required CreateWorkItemModel Model { get; set; }

	//public required List<SelectListItem> Options { get; set; }
	public async Task OnGet()
	{
		logger.LogInformation("getting clients");
		var clients = await context.Clients.ToListAsync();
		// Options = clients.Select(c => new SelectListItem
		// {
		// 	Value = c.Id.ToString(),
		// 	Text = c.Name
		// }).ToList();
		Model.Clients = clients;

	}

	public async Task<IActionResult> OnPost()
	{
		logger.LogInformation("{model}", Model.ToJson());
		if (!ModelState.IsValid)
		{
			logger.LogWarning("invalid model {@model}", Model);
			return Page();
		}

		//convert the work
		logger.LogInformation("converting {@}", Model);
		var item = Model.Adapt<WorkItem>();
		item.Id = NewId.NextSequentialGuid();
		logger.LogInformation("item {@}", item);
		context.WorkItems.Add(item);

		await context.SaveChangesAsync();

		return RedirectToPage("Index");
	}
}
