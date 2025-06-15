using Htmx;
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

	public required List<WorkItemIndexModel> Hours { get; set; }

	[BindProperty(SupportsGet = true)]
	public string Search { get; set; }

	public int PageNum { get; set; } = 1;
	public int PageSize { get; set; } = 20;


	public async Task<IActionResult> OnGet(string? search, int? pageNum, int? pageSize)
	{
		PageSize = pageSize ?? 20;
		PageNum = pageNum ?? 1;
		var skip = (PageNum - 1) * PageSize;
		Search = search ?? string.Empty;

		var hours = context.WorkItems.Include(w => w.Client).AsQueryable();

		if (!string.IsNullOrWhiteSpace(Search))
		{
			//this isn't ideal.  If we keep to pg, then usen EF.Function.Ilike but i currently
			//plan to switch to sqlite so this may still be need to be kept
			hours = hours.Where(c =>
				c.Description.ToLower().Contains(Search.ToLower()) ||
				c.Client.Name.ToLower().Contains(Search.ToLower())
				);
		}

		var hoursPaged = await hours.OrderBy(c => c.Date).Skip(skip).Take(PageSize).ToListAsync();

		Hours = hoursPaged.Adapt<List<WorkItemIndexModel>>();

		return Request.IsHtmx()
			? Partial("_WorkItemsRows", this)
			: Page();

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
