using Microsoft.AspNetCore.Mvc.Rendering;

namespace Invoicer.Web.Pages.Invoices
{
	public class InvoiceEditModel : InvoiceIndexModel
	{
		public InvoiceEditModel()
		{
			Hours = [];
		}
		public List<Entities.Hours> Hours { get; set; }
		public List<SelectListItem> Statuses { get; set; }
	}
}

