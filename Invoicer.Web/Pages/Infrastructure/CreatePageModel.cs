using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mapster;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MassTransit;

namespace Invoicer.Web.Pages
{
    public class CreatePageModel<TModel, TEntity>: PageModel, 
    IGetByIdPageModel<TModel, TEntity>,
    IPostPageModel<TEntity>   
    where TEntity : Entity
    where TModel : IModel
    {
        private readonly DataContext context;

        public CreatePageModel(DataContext context, ILogger<CreatePageModel<TModel, TEntity>> logger)
        {
            this.context = context;
            Logger = logger;
        }

        [BindProperty]
        public TModel Model {get;set;}
        public ILogger<CreatePageModel<TModel, TEntity>> Logger { get; }

        public virtual async Task<IActionResult> OnGet(Guid id)
        {
            var entity = await context.Set<TEntity>().FirstOrDefaultAsync(c => c.Id == id);
            Logger.LogInformation("{entity}", entity);

            if (entity == null)
                return Page();
            Model = entity.Adapt<TModel>();
            return Page();
        }

        public virtual async Task<IActionResult> OnPost()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }
            //convert to an new entity
            TEntity entity = Model.Adapt<TEntity>(); 
            //create a sequential uuid
            entity.Id = NewId.NextSequentialGuid();   
            context.Add(entity);
            await context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
