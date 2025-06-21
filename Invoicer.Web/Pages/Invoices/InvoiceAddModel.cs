using Microsoft.AspNetCore.Mvc.Rendering;

namespace Invoicer.Web.Pages.Invoices
{
	public class InvoiceAddModel
	{
		public Guid ClientId { get; set; }
		public Guid AccountId { get; set; }
		public DateTime UpdatedAt { get; set; }
		public List<HoursAddModel> SelectedHours { get; set; } = [];
		public required List<InvoiceHoursModel> OutStandingHours { get; set; }
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

