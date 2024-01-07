namespace Invoicer.Web.Pages.Invoices;
using Invoicer.Web.Pages.WorkItems;

public class InvoicesIndexModel 
{
    public InvoicesIndexModel()
    {
		Invoices = new List<Invoice>();
		OutstandingWorkItems = new List<WorkItem>();
    }

    public List<Invoice> Invoices {get;set;}	
	public List<WorkItem> OutstandingWorkItems {get;set;}
}

