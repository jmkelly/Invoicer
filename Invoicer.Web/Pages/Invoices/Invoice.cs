using Invoicer.Web.Pages.Clients.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Invoicer.Web.Pages.Invoices;

public class Invoice : Entity
{
	public Invoice()
	{
		Hours = [];
		CreatedAt = DateTime.UtcNow;
		InvoiceDate = DateTime.UtcNow;
		UpdatedAt = DateTime.UtcNow;
	}
	public List<Entities.Hours> Hours { get; set; }
	[Required]
	[StringLength(50)]
	public required string InvoiceCode { get; set; }
	[Required]
	public required Client Client { get; set; }
	[Required]
	public required DateTime CreatedAt { get; set; }
	[Required]
	public required DateTime InvoiceDate { get; set; }
	public DateTime UpdatedAt { get; set; }

	[Required]
	public required InvoiceStatus InvoiceStatus { get; set; }
	[Required]
	public required MyAccount.MyAccount Account { get; set; }

	public decimal Total()
	{
		if (Hours == null || !Hours.Any())
			return 0;
		return Hours.Sum(c => c.Total());
	}
	public bool IsAllowedToBeDeleted()
	{
		return InvoiceStatus == InvoiceStatus.Created;
	}

	public void RemoveAllHours()
	{
		Hours?.Clear();
	}

	public Result AddHours(Entities.Hours hours)
	{
		if (hours == null)
			return Result.Failure("Hours cannot be null");
			
		if (hours.ClientId != Client.Id)
			return Result.Failure("Work client differs from the invoice client, multiple clients cannot be on same invoice. Create a separate invoice for each client.");

		Hours.Add(hours);
		UpdatedAt = DateTime.UtcNow;
		return Result.Success();
	}
}
