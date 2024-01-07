namespace Invoicer.Web.Pages.Invoices
{
    public class WorkItemInvoiceModel
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }
        public decimal Total { get; set; }
        public string? ClientName { get; set; }
        public Guid ClientId { get; set; }

        public string Label()
        {
            return $"{ClientName} - {Description} (${Total})";
        }

        public bool IsSelected { get; set; }
    }
}

