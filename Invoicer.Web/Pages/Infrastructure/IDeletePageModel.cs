using Microsoft.AspNetCore.Mvc;

namespace Invoicer.Web.Pages
{
    public interface IDeleteByIdPageModel<TEntity> where TEntity: Entity
    {
        Task<IActionResult> OnPost(Guid id);
    }
}
