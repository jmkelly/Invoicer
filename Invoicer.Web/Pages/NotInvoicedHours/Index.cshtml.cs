using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Invoicer.Web.Pages.Invoices;
using Invoicer.Web.Pages.MyAccount;
using Htmx;

namespace Invoicer.Web.Pages.NotInvoicedHours
{
    public class IndexModel : PagedModel
    {
        private readonly SqliteContext _context;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(SqliteContext context, ILogger<IndexModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<Entities.Hours> NotInvoicedHours { get; set; } = new();
        public List<Invoicer.Web.Pages.MyAccount.MyAccount> Accounts { get; set; } = new();

        [BindProperty]
        public Guid SelectedAccountId { get; set; }

        [BindProperty(SupportsGet = true)]
        public SelectedClientIdModel SelectedClient { get; set; } = new();
        public async Task<IActionResult> OnGetAsync(string? search, int? pageNum, int? pageSize, Guid? clientId)
        {
            SetPaging(search, pageNum, pageSize);
            var query = _context.Hours
                .Include(h => h.Client)
                .Where(h => h.Invoice == null);

            if (!string.IsNullOrWhiteSpace(Search))
            {
                var searchLower = Search.ToLower();
                query = query.Where(h =>
                    (h.Client != null && h.Client.Name.ToLower().Contains(searchLower)) ||
                    (h.Description != null && h.Description.ToLower().Contains(searchLower)));
            }

            if (clientId.HasValue)
            {
                query = query.Where(h => h.ClientId == clientId.Value);
            }

            NotInvoicedHours = await query
                .OrderBy(h => h.Client.Name)
                .ThenBy(h => h.Date)
                .Skip(Skip)
                .Take(PageSize)
                .ToListAsync();

            if (clientId.HasValue && NotInvoicedHours.Count > 0)
            {
                SelectedClient.SelectedClientId = clientId.Value;
                SelectedClient.ClientName = NotInvoicedHours[0].Client?.Name ?? string.Empty;
            }

            Accounts = await _context.MyAccounts.ToListAsync();
            return Request.IsHtmx()
                        ? Partial("_NotInvoicedHoursRows", this)
                        : Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var hour = await _context.Hours.FindAsync(id);

            if (hour == null)
            {
                return NotFound();
            }

            _context.Hours.Remove(hour);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Deleted hour entry with ID: {Id}", id);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCreateInvoiceAsync([FromForm] List<Guid> selectedHours, [FromForm] Guid SelectedAccountId)
        {
            if (selectedHours == null || !selectedHours.Any())
            {
                TempData["Error"] = "No hours selected.";
                return RedirectToPage();
            }
            if (SelectedAccountId == Guid.Empty)
            {
                TempData["Error"] = "No account selected.";
                return RedirectToPage();
            }
            var hours = await _context.Hours
                .Include(h => h.Client)
                .Where(h => selectedHours.Contains(h.Id))
                .ToListAsync();
            var clientIds = hours.Select(h => h.ClientId).Distinct().ToList();
            if (clientIds.Count > 1)
            {
                TempData["Error"] = "Multiple clients selected. An invoice can only be created for one client at a time.";
                return RedirectToPage();
            }
            var clientId = clientIds.First();
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == clientId);
            if (client == null)
            {
                TempData["Error"] = $"No client found with id {clientId}";
                return RedirectToPage();
            }
            var account = await _context.MyAccounts.FirstOrDefaultAsync(a => a.Id == SelectedAccountId);
            if (account == null)
            {
                TempData["Error"] = $"No account found with id {SelectedAccountId}";
                return RedirectToPage();
            }
            var createdAt = DateTime.UtcNow;
            var invoice = new Invoice
            {
                Client = client,
                Id = MassTransit.NewId.NextSequentialGuid(),
                CreatedAt = createdAt,
                InvoiceDate = createdAt,
                InvoiceStatus = InvoiceStatus.Created,
                InvoiceCode = InvoiceCodeGenerator.CreateInvoiceCode(client.ClientCode, createdAt),
                Account = account,
                UpddatedAt = createdAt
            };
            foreach (var wi in hours)
            {
                var result = invoice.AddHours(wi);
                if (!result.IsSuccess)
                {
                    TempData["Error"] = result.Error;
                    return RedirectToPage();
                }
            }
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Invoice created successfully.";
            return RedirectToPage("/Invoices/Index");
        }

        public async Task<IActionResult> OnGetClearClientFilterAsync(string? search, int? pageNum, int? pageSize)
        {
            return await OnGetAsync(search, pageNum, pageSize, null);
        }
    }

    public class SelectedClientIdModel
    {
        public Guid? SelectedClientId { get; set; }
        public string ClientName { get; set; } = string.Empty;
    }
}