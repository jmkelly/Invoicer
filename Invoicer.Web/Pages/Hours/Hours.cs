using Invoicer.Web.Pages.Invoices;
using Invoicer.Web.Pages.Clients.Models;
using Invoicer.Web.Pages.Hours;

namespace Invoicer.Web.Entities;

public class Hours : Entity
{
	private const int HoursInDay = 24;
	private const int WorkHoursInDay = 8;
	public DateOnly Date { get; set; }
	public decimal NumberOfHours { get; set; }
	public required string Description { get; set; }
	public decimal Rate { get; set; }
	public RateUnits RateUnits { get; set; }
	public Guid ClientId { get; set; }
	public DateTime DateRecorded { get; set; }
	public Client? Client { get; set; }
	public decimal Total()
	{
		if (RateUnits == RateUnits.PerHour)
		{
			return Rate * (decimal)NumberOfHours;
		}

		if (RateUnits == RateUnits.PerDay)
		{
			//we assume an 8hr day
			return Rate * (decimal)NumberOfHours / HoursInDay / WorkHoursInDay;
		}

		throw new NotImplementedException("unknown rate units");
	}
	public Invoice? Invoice { get; set; }

	public bool HasInvoice()
	{
		return Invoice != null;
	}
}
