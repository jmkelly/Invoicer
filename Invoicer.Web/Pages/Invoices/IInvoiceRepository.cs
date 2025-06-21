namespace Invoicer.Web.Pages.Invoices
{
    public interface IInvoiceRepository
    {
        Task<Invoice?> GetByIdAsync(Guid id);
        Task<IEnumerable<Invoice>> GetAllAsync();
        Task<IEnumerable<Invoice>> GetByClientAsync(Guid clientId);
        Task<IEnumerable<Invoice>> GetByStatusAsync(InvoiceStatus status);
        Task<Invoice> AddAsync(Invoice invoice);
        Task<Invoice> UpdateAsync(Invoice invoice);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<int> SaveChangesAsync();
        Task<IEnumerable<Invoice>> SearchAsync(string searchTerm, int skip, int take);
        Task<int> GetTotalCountAsync();
    }
}

