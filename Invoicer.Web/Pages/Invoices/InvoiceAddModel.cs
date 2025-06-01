using Microsoft.AspNetCore.Mvc.Rendering;

namespace Invoicer.Web.Pages.Invoices
{
	public class InvoiceAddModel
	{
		public Guid ClientId { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpddatedAt { get; set; }
		public required List<WorkItemInvoiceModel> SelectedWorkItems { get; set; }
		public required List<WorkItemInvoiceModel> OutStandingWorkItems { get; set; }
		public required List<MyAccount.MyAccount> Accounts { get; set; }
		public Guid SelectedAccountId { get; set; }
	}

	public static class InvoiceAddModelExtensions
	{
		public static List<SelectListItem> ToSelectItemList(this List<MyAccount.MyAccount> accounts)
		{
			return accounts.Select(n => new SelectListItem { Text = n.Label(), Value = n.Id.ToString() }).ToList();
		}
	}
}

