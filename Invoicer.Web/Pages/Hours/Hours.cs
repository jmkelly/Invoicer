using Invoicer.Web.Pages.Invoices;
using Invoicer.Web.Pages.Clients.Models;
using Invoicer.Web.Pages.Hours;
using System.ComponentModel.DataAnnotations;

namespace Invoicer.Web.Entities;

public class Hours : Entity
{
	private const int HoursInDay = 24;
	private const int WorkHoursInDay = 8;
	
	[Required]
	public DateOnly Date { get; set; }
	
	[Range(0, 24, ErrorMessage = "NumberOfHours must be between 0 and 24")]
	public decimal NumberOfHours { get; set; }
	
	[Required]
	[StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
	public required string Description { get; set; }
	
	[Range(0, double.MaxValue, ErrorMessage = "Rate must be positive")]
	public decimal Rate { get; set; }
	
	[Required]
	public RateUnits RateUnits { get; set; }
	
	[Required]
	public Guid ClientId { get; set; }
	
	[Required]
	public DateTime DateRecorded { get; set; }
	
	public Client? Client { get; set; }
	
	public decimal Total()
	{
		if (NumberOfHours <= 0 || Rate <= 0)
			return 0;
			
		return RateUnits switch
		{
			RateUnits.PerHour => Rate * NumberOfHours,
			RateUnits.PerDay => Rate * (NumberOfHours / WorkHoursInDay),
			_ => throw new ArgumentException($"Unknown rate units: {RateUnits}")
		};
	}
	
	public Invoice? Invoice { get; set; }

	public bool HasInvoice()
	{
		return Invoice != null;
	}
}
