using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Invoicer.Web.Pages.MyAccount
{
    public class Create : CreatePageModel<MyAccountCreateModel, MyAccount>
    {
        public Create(DataContext context, ILogger<CreatePageModel<MyAccountCreateModel, MyAccount>> logger) : base(context, logger)
        {
        }
    }
}
