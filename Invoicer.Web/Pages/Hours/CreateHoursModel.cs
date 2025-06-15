using Invoicer.Web.Pages.Clients.Models;

namespace Invoicer.Web.Pages.Hours;

public class CreateHoursModel
{
	public DateOnly Date { get; set; }
	public decimal NumberOfHours { get; set; }
	public required string Description { get; set; }
	public decimal Rate { get; set; }
	public RateUnits RateUnits { get; set; }
	public Guid ClientId { get; set; }
	public required List<Client> Clients { get; set; }
}
