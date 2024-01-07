namespace Invoicer.Web.Pages.Invoices
{
    public interface IInvoiceRepository
    {
        Task Remove(Guid id);
        Task SaveChangesAsync();
    }
}

