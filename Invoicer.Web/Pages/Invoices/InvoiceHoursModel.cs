namespace Invoicer.Web.Pages.Invoices
{
	public class InvoiceHoursModel
	{
		public Guid Id { get; set; }
		public string? Description { get; set; }
		public decimal Total { get; set; }
		public string? ClientName { get; set; }
		public Guid? ClientId { get; set; }

		public string Label()
		{
			return $"{ClientName} - {Description} (${Total:F2})";
		}

		public bool IsSelected { get; set; }
	}
}

