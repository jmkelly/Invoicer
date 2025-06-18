using Mapster;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Invoicer.Web.Pages.Settings
{
	public class Index : PageModel
	{
		[BindProperty]
		public required SettingModel Model { get; set; }

		public Index(SqliteContext context, ILogger<Index> logger)
		{
			Context = context;
			Logger = logger;
		}

		public SqliteContext Context { get; }
		public ILogger<Index> Logger { get; }

		public async Task<IActionResult> OnGet()
		{
			var entity = await Context.Settings.FirstOrDefaultAsync();

			if (entity != null)
			{
				Model = entity.Adapt<SettingModel>();
			}
			Logger.LogInformation("{entity}", entity);

			return Page();
		}

		public async Task<IActionResult> OnPost()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			var entity = Model.Adapt<Setting>();

			if (Model.Id == default)
			{
				Logger.LogInformation("no Id for {@entity}, new entity ", entity);
				entity.Id = NewId.NextSequentialGuid();
				Context.Add(entity);
			}
			else
			{
				Logger.LogInformation("{@entity} exists updating", entity);
				Context.Update(entity);
			}

			await Context.SaveChangesAsync();
			return RedirectToPage("./Index");
		}

	}
}
