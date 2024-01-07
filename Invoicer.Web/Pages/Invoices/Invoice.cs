using Invoicer.Web.Pages.Clients.Models;
using Invoicer.Web.Pages.WorkItems;
using Microsoft.EntityFrameworkCore;

namespace Invoicer.Web.Pages.Invoices;

public class Invoice : Entity
{
    public Invoice()
    {
        WorkItems = new List<WorkItem>();
    }
    public List<WorkItem> WorkItems { get; set; }
    public required string InvoiceCode { get; set; }
    public required Client Client { get; set; }
    public required DateTime CreatedAt { get; set; }
    public DateTime UpddatedAt { get; set; }

    public required InvoiceStatus InvoiceStatus { get; set; }
    public MyAccount.MyAccount Account { get; set; }

    public decimal Total()
    {
        if (WorkItems == null)
            return 0;
        return WorkItems.Sum(c => c.Total());
    }
    public bool IsAllowedToBeDeleted()
    {
        if (InvoiceStatus == InvoiceStatus.Created)
            return true;
        return false;
    }

    public static string CreateInvoiceCode(string clientName, DateTime dateTime)
    {
        //create a code based on the year, date, day
        return $"{clientName}:{dateTime.Year:D2}{dateTime.Month:D2}{dateTime.Day:D2}-{dateTime.Hour:D2}{dateTime.Minute:D2}{dateTime.Second:D2}";
    }

    public void RemoveAllWorkItems()
    {
        WorkItems.Clear();
    }

    public Result AddWorkItem(WorkItem workItem)
    {
        if (workItem.ClientId != Client.Id)
            return Result.Failure("Work client differs from the invoice client, muliple clients cannot be on same invoice.  Create a seperate invoice for each client.");

        WorkItems.Add(workItem);
        UpddatedAt = DateTime.UtcNow;
        return Result.Success();
    }
}
