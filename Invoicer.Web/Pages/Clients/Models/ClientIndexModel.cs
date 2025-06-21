using Invoicer.Web.Pages.Clients.Models;

namespace Invoicer.Web.Pages.Clients
{
	public class ClientIndexModel
	{
		public Guid Id { get; private set; }
		public string CompanyName { get; private set; }
		public required string Name { get; set; }
		public required string ClientCode { get; set; }
		public string Address { get; private set; }

		public static ClientIndexModel Create(Client client)
		{
			return new ClientIndexModel
			{
				Id = client.Id,
				CompanyName = client.CompanyName ?? client.Name,
				Name = client.Name,
				ClientCode = client.ClientCode,
				Address = $"{client.StreetNumber} {client.Street} {client.City}, {client.State} {client.Postcode}"
			};
		}

	}
}
