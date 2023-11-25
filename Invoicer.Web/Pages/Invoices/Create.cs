using System.Runtime.Intrinsics.X86;
using Invoicer.Web.Pages.Clients.Models;
using Invoicer.Web.Pages.Invoices;
using Invoicer.Web.Pages.WorkItems;
using Mapster;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Invoicer.Web.Pages.Invoices
{
    public class Create : PageModel
    {
        private readonly DataContext context;
        private readonly ILogger<Create> logger;

        public Create(DataContext context, ILogger<Create> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public InvoiceAddModel Model { get; set; }
        public List<WorkItemInvoiceModel> OutStandingWorkItems { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var outStandingWorkItems = await context.WorkItems
            .Include(c => c.Client)
            .Where(c => c.Invoice == null).ToListAsync();

            logger.LogInformation("outstanding work items {OutStandingWorkItems}", outStandingWorkItems.Count);

            OutStandingWorkItems = outStandingWorkItems.Select(o => new WorkItemInvoiceModel{
                Id = o.Id,
                Description = o.Description,
                Total = o.Total(),
                ClientName = o.Client.Name,
                ClientId = o.Client.Id,
                IsSelected = true //set selected by default
            }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPost(List<WorkItemAddModel> workItems)
        {
            logger.LogInformation("getting into post");
            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                logger.LogInformation("message {message}", message);
                TempData.AddMessage(message);
                return await OnGet();
            }

            //get the selected work items from database
            workItems = workItems.Where(c => c.IsSelected).ToList();
            var selectedWorkItems = await context.WorkItems.Where(c => workItems.Any(w => w.Id  == c.Id)).ToListAsync();
            //Model.SelectedWorkItems = workItems;

            //get the client
            var client = await context.Clients.FirstOrDefaultAsync(c => c.Id == Model.ClientId);
            if (client == null)
            {
                logger.LogWarning("No client found iwth id {clientId}", Model.ClientId);
                TempData.AddMessage($"No client found with id {Model.ClientId}");
                return RedirectToPage("Create");
            }

            logger.LogInformation("getting into post");
            var createdAt = DateTime.UtcNow;
            var invoice = new Invoice
            {
                //create a sequential uuid
                Client = client,
                Id = NewId.NextSequentialGuid(),
                CreatedAt = createdAt,
                InvoiceStatus = InvoiceStatus.Created,
                InvoiceCode = Invoice.CreateInvoiceCode(
                    client.Name, 
                    createdAt
                )
            };

            foreach (var wi in Model.SelectedWorkItems)
            {
                //add work to invoice
                //get the work item from the db
                var item = await context.WorkItems.FirstOrDefaultAsync(w => w.Id == wi.Id);
                if (item == null)
                {
                    logger.LogWarning("No client found with id {clientId}", Model.ClientId);
                    TempData.AddMessage("No Work item found...");
                    return RedirectToPage("Create");
                }
                var result = invoice.AddWorkItem(item);
                if (!result.IsSuccess)
                {
                    TempData.Add("error", result.Error);
                    return RedirectToPage("Create");
                }
            }
            context.Add(invoice);
            await context.SaveChangesAsync();

            return RedirectToPage("Index");
        }


    }

    public class WorkItemAddModel
    {
        public Guid Id {get;set;}
        public bool IsSelected {get;set;}
    }
    public class WorkItemInvoiceModel 
    {
        public Guid Id {get;set;}
        public string? Description {get;set;}
        public decimal Total {get;set;}
        public string? ClientName {get;set;}
        public Guid ClientId {get;set;}
 
        public string Label()
        {
            return $"{ClientName} - {Description} (${Total})";
        }

        public bool IsSelected {get;set;}
    }

    public class InvoiceAddModel
    { 
        public Guid ClientId {get;set;}
        public DateTime CreatedAt {get;set;}
        public DateTime UpddatedAt {get;set;}
        public required List<WorkItemInvoiceModel> SelectedWorkItems {get;set;}
    }
}
