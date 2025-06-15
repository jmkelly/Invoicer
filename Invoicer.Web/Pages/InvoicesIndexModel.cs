namespace Invoicer.Web.Pages;

using Invoicer.Web.Pages.Invoices;
using Invoicer.Web.Pages.Hours;

public class IndexViewModel
{
	public IndexViewModel()
	{
		Invoices = new List<Invoice>();
		OutstandingHours = [];
	}

	public List<Invoice> Invoices { get; set; }
	public List<Entities.Hours> OutstandingHours { get; set; }
	public CreateHoursModel NewHours { get; set; }
	public InvoiceAddModel NewInvoice { get; set; }
}

