using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mapster;
using Invoicer.Web.Extensions;

namespace Invoicer.Web.Pages
{
	public class EditPageModel<TModel, TEntity> : CreatePageModel<TModel, TEntity>
	where TEntity : Entity
	where TModel : IModel
	{
		public EditPageModel(DataContext context, ILogger<CreatePageModel<TModel, TEntity>> logger) : base(context, logger)
		{
		}

		public override async Task<IActionResult> OnPost()
		{
			Logger.LogInformation("Edit {json}", Model.ToJson());

			if (!ModelState.IsValid)
			{
				Logger.LogWarning("Model {@Model} not valid", Model);
				return Page();
			}
			//convert to an new entity
			TEntity entity = Model.Adapt<TEntity>();
			Context.Entry(entity).State = EntityState.Modified;
			await Context.SaveChangesAsync();

			return RedirectToPage("Index");
		}
	}
}

