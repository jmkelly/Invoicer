using Microsoft.AspNetCore.Mvc;

namespace Invoicer.Web.Pages
{
    public interface IGetByIdPageModel<TModel, TEntity> 
        where TEntity: Entity 
        where TModel : IModel
    {
        Task<IActionResult> OnGet(Guid id);
    }
}
