namespace Invoicer.Web.Pages.Clients
{
	public class ClientDeleteModel : IModel
	{
		public Guid Id { get; set; }
		public required string CompanyName { get; set; }
		public required string Name { get; set; }

	}
}
