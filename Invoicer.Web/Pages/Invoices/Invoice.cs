using Invoicer.Web.Pages.Clients.Models;
using Microsoft.EntityFrameworkCore;

namespace Invoicer.Web.Pages.Invoices;

public class Invoice : Entity
{
	public Invoice()
	{
		Hours = [];
		CreatedAt = DateTime.UtcNow;
		InvoiceDate = DateTime.UtcNow;
	}
	public List<Entities.Hours> Hours { get; set; }
	public required string InvoiceCode { get; set; }
	public required Client Client { get; set; }
	public required DateTime CreatedAt { get; set; }
	public required DateTime InvoiceDate { get; set; }
	public DateTime UpddatedAt { get; set; }

	public required InvoiceStatus InvoiceStatus { get; set; }
	public required MyAccount.MyAccount Account { get; set; }

	public decimal Total()
	{
		if (Hours == null)
			return 0;
		return Hours.Sum(c => c.Total());
	}
	public bool IsAllowedToBeDeleted()
	{
		if (InvoiceStatus == InvoiceStatus.Created)
			return true;
		return false;
	}

	public void RemoveAllHours()
	{
		Hours.Clear();
	}

	public Result AddHours(Entities.Hours hours)
	{
		if (hours.ClientId != Client.Id)
			return Result.Failure("Work client differs from the invoice client, muliple clients cannot be on same invoice.  Create a seperate invoice for each client.");

		Hours.Add(hours);
		UpddatedAt = DateTime.UtcNow;
		return Result.Success();
	}
}
