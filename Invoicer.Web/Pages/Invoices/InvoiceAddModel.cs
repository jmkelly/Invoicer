namespace Invoicer.Web.Pages.Invoices
{
    public class InvoiceAddModel
    {
        public Guid ClientId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpddatedAt { get; set; }
        public required List<WorkItemInvoiceModel> SelectedWorkItems { get; set; }
    }
}

