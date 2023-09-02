using Invoicer.Web.Pages.Clients.Models;
using Microsoft.AspNetCore.Mvc;

namespace Invoicer.Web.Pages.Clients
{

    public class Delete : DeletePageModel<ClientDeleteModel, Client>
    {
        public Delete(DataContext context, ILogger<DeletePageModel<ClientDeleteModel, Client>> logger) : base(context, logger)
        {
        }
    }
}
