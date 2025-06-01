using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Invoicer.Web.Pages.WorkItems;

public class IndexModel : PageModel
{
	private readonly ILogger<IndexModel> _logger;
	private readonly DataContext context;

	public IndexModel(ILogger<IndexModel> logger, DataContext context)
	{
		_logger = logger;
		this.context = context;
	}

	public required List<WorkItemIndexModel> WorkItems { get; set; }

	public async Task OnGet()
	{
		var workitems = await context
		.WorkItems
		.Include(w => w.Client)
		.ToListAsync();
		WorkItems = workitems.Adapt<List<WorkItemIndexModel>>();
	}

	public async Task<ActionResult> OnGetDelete(Guid id)
	{
		var wi = await context.WorkItems.FirstOrDefaultAsync(w => w.Id == id);
		if (wi is null)
		{
			return new NoContentResult();
		}
		context.WorkItems.Remove(wi);
		await context.SaveChangesAsync();
		return new OkResult();
	}

}
public class WorkItemIndexModel : WorkItem
{

}
