using Invoicer.Web.Pages.WorkItems;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Invoicer.Web.Pages.Invoices
{
	public class InvoiceEditModel : InvoiceIndexModel
	{
		public InvoiceEditModel()
		{
			Hours = [];
		}
		public List<WorkItem> Hours { get; set; }
		public List<SelectListItem> Statuses { get; set; }
	}
}

