using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mapster;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Invoicer.Web.Pages
{
	public class DeletePageModel<TModel, TEntity> : PageModel,
	IGetByIdPageModel<TModel, TEntity>,
	IDeleteByIdPageModel<TEntity>
	where TEntity : Entity
	where TModel : IModel
	{
		private readonly DataContext context;

		public DeletePageModel(DataContext context, ILogger<DeletePageModel<TModel, TEntity>> logger)
		{
			this.context = context;
			Logger = logger;
		}

		[BindProperty]
		public required TModel Model { get; set; }
		public ILogger<DeletePageModel<TModel, TEntity>> Logger { get; }

		public virtual async Task<IActionResult> OnGet(Guid id)
		{
			var entity = await context.Set<TEntity>().FirstOrDefaultAsync(c => c.Id == id);
			Logger.LogInformation("{entity}", entity);

			if (entity == null)
				return Page();
			Model = entity.Adapt<TModel>();
			return Page();
		}

		public virtual async Task<IActionResult> OnPost(Guid id)
		{

			var entity = await context.Set<TEntity>().FirstOrDefaultAsync(c => c.Id == id);
			if (entity == null)
			{
				return RedirectToPage("Index");
			}

			context.Remove(entity);
			await context.SaveChangesAsync();

			return RedirectToPage("Index");
		}
	}
}
