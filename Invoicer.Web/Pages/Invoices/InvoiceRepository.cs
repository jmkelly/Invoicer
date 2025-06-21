using Microsoft.EntityFrameworkCore;

namespace Invoicer.Web.Pages.Invoices
{
	public class InvoiceRepository : IInvoiceRepository
	{
		private readonly SqliteContext _context;
		private readonly ILogger<InvoiceRepository> _logger;

		public InvoiceRepository(SqliteContext context, ILogger<InvoiceRepository> logger)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<Invoice?> GetByIdAsync(Guid id)
		{
			try
			{
				return await _context.Invoices
					.Include(i => i.Hours)
					.Include(i => i.Client)
					.Include(i => i.Account)
					.FirstOrDefaultAsync(i => i.Id == id);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error retrieving invoice with ID {InvoiceId}", id);
				throw;
			}
		}

		public async Task<IEnumerable<Invoice>> GetAllAsync()
		{
			try
			{
				return await _context.Invoices
					.Include(i => i.Hours)
					.Include(i => i.Client)
					.Include(i => i.Account)
					.OrderByDescending(i => i.CreatedAt)
					.ToListAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error retrieving all invoices");
				throw;
			}
		}

		public async Task<IEnumerable<Invoice>> GetByClientAsync(Guid clientId)
		{
			try
			{
				return await _context.Invoices
					.Include(i => i.Hours)
					.Include(i => i.Client)
					.Include(i => i.Account)
					.Where(i => i.Client.Id == clientId)
					.OrderByDescending(i => i.CreatedAt)
					.ToListAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error retrieving invoices for client {ClientId}", clientId);
				throw;
			}
		}

		public async Task<IEnumerable<Invoice>> GetByStatusAsync(InvoiceStatus status)
		{
			try
			{
				return await _context.Invoices
					.Include(i => i.Hours)
					.Include(i => i.Client)
					.Include(i => i.Account)
					.Where(i => i.InvoiceStatus == status)
					.OrderByDescending(i => i.CreatedAt)
					.ToListAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error retrieving invoices with status {Status}", status);
				throw;
			}
		}

		public async Task<Invoice> AddAsync(Invoice invoice)
		{
			try
			{
				if (invoice == null)
					throw new ArgumentNullException(nameof(invoice));

				_context.Invoices.Add(invoice);
				await _context.SaveChangesAsync();
				
				_logger.LogInformation("Added invoice with ID {InvoiceId}", invoice.Id);
				return invoice;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error adding invoice");
				throw;
			}
		}

		public async Task<Invoice> UpdateAsync(Invoice invoice)
		{
			try
			{
				if (invoice == null)
					throw new ArgumentNullException(nameof(invoice));

				invoice.UpdatedAt = DateTime.UtcNow;
				_context.Invoices.Update(invoice);
				await _context.SaveChangesAsync();
				
				_logger.LogInformation("Updated invoice with ID {InvoiceId}", invoice.Id);
				return invoice;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error updating invoice with ID {InvoiceId}", invoice?.Id);
				throw;
			}
		}

		public async Task DeleteAsync(Guid id)
		{
			try
			{
				var invoice = await GetByIdAsync(id);
				if (invoice == null)
				{
					_logger.LogWarning("Invoice with ID {InvoiceId} not found for deletion", id);
					return;
				}

				if (!invoice.IsAllowedToBeDeleted())
				{
					throw new InvalidOperationException($"Invoice with ID {id} cannot be deleted in its current status");
				}

				_context.Invoices.Remove(invoice);
				await _context.SaveChangesAsync();
				
				_logger.LogInformation("Deleted invoice with ID {InvoiceId}", id);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error deleting invoice with ID {InvoiceId}", id);
				throw;
			}
		}

		public async Task<bool> ExistsAsync(Guid id)
		{
			try
			{
				return await _context.Invoices.AnyAsync(i => i.Id == id);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error checking existence of invoice with ID {InvoiceId}", id);
				throw;
			}
		}

		public async Task<int> SaveChangesAsync()
		{
			try
			{
				return await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error saving changes to database");
				throw;
			}
		}

		public async Task<IEnumerable<Invoice>> SearchAsync(string searchTerm, int skip, int take)
		{
			try
			{
				var query = _context.Invoices
					.Include(i => i.Hours)
					.Include(i => i.Client)
					.Include(i => i.Account)
					.AsQueryable();

				if (!string.IsNullOrWhiteSpace(searchTerm))
				{
					var searchLower = searchTerm.ToLower();
					query = query.Where(i =>
						i.InvoiceCode.ToLower().Contains(searchLower) ||
						i.Client.Name.ToLower().Contains(searchLower)
					);
				}

				return await query
					.OrderByDescending(i => i.CreatedAt)
					.Skip(skip)
					.Take(take)
					.ToListAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error searching invoices with term '{SearchTerm}'", searchTerm);
				throw;
			}
		}

		public async Task<int> GetTotalCountAsync()
		{
			try
			{
				return await _context.Invoices.CountAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error getting total invoice count");
				throw;
			}
		}
	}
}

