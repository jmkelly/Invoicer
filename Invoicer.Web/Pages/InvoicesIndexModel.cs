namespace Invoicer.Web.Pages;

using Invoicer.Web.Pages.Invoices;
using Invoicer.Web.Pages.WorkItems;

public class IndexViewModel
{
	public IndexViewModel()
	{
		Invoices = new List<Invoice>();
		OutstandingWorkItems = new List<WorkItem>();
	}

	public List<Invoice> Invoices { get; set; }
	public List<WorkItem> OutstandingWorkItems { get; set; }
	public required CreateWorkItemModel NewWorkItem { get; set; }
}

