namespace Invoicer.Web.Pages.Clients.Models
{
	public class Client : Entity
	{
		public string? CompanyName { get; set; }
		public required string Name { get; set; }
		public required string ClientCode { get; set; }
		public string? BSB { get; set; }
		public string? AccountNo { get; set; }
		public string? StreetNumber { get; set; }
		public string? Street { get; set; }
		public string? City { get; set; }
		public string? State { get; set; }
		public string? Postcode { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
		public DateTimeOffset UpdatedAt { get; set; }

		public string SearchString => $"{CompanyName} {Name} {BSB} {AccountNo} {StreetNumber} {Street} {City} {State} {Postcode}".ToLowerInvariant();
	}
}
