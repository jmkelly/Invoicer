using Microsoft.AspNetCore.Mvc;

namespace Invoicer.Web.Pages
{
    public interface IPostPageModel<TEntity> where TEntity : Entity
    {
          Task<IActionResult> OnPost();
    }
}
