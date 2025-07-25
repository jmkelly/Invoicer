using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mapster;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MassTransit;
using Invoicer.Web.Extensions;

namespace Invoicer.Web.Pages
{

	public class CreatePageModel<TModel, TEntity> : PageModel,
	IGetByIdPageModel<TModel, TEntity>,
	IPostPageModel<TEntity>
	where TEntity : Entity
	where TModel : IModel
	{
		public readonly SqliteContext Context;

		public CreatePageModel(SqliteContext context, ILogger<CreatePageModel<TModel, TEntity>> logger)
		{
			Context = context;
			Logger = logger;
		}

		[BindProperty]
		public required TModel Model { get; set; }
		public ILogger<CreatePageModel<TModel, TEntity>> Logger { get; }

		public virtual async Task<IActionResult> OnGet(Guid id)
		{
			var entity = await Context.Set<TEntity>().FirstOrDefaultAsync(c => c.Id == id);
			Logger.LogInformation("Get {entity}", entity);

			if (entity == null)
				return Page();
			Model = entity.Adapt<TModel>();
			return Page();
		}

		public virtual async Task<IActionResult> OnPost()
		{
			Logger.LogInformation("Create {json}", Model.ToJson());
			Logger.LogInformation("Create {entity}", Model);

			if (!ModelState.IsValid)
			{
				Logger.LogWarning("Model {@Model} not valid", Model);
				return Page();
			}
			//convert to an new entity
			TEntity entity = Model.Adapt<TEntity>();
			//create a sequential uuid
			entity.Id = NewId.NextSequentialGuid();
			Context.Add(entity);
			await Context.SaveChangesAsync();

			return RedirectToPage("Index");
		}
	}
}
