using Invoicer.Web.Pages.Clients.Models;
using MassTransit;

public class SeedDataGenerator
{
	private static readonly Random random = new();

	// Arrays for generating realistic-ish data
	private static readonly string[] CompanySuffixes = { "Pty Ltd", "Solutions", "Innovations", "Group", "Enterprises", "Consulting", "Tech", "Services", "Global", "Partners" };
	private static readonly string[] FirstNames = { "Alice", "Bob", "Charlie", "David", "Eve", "Frank", "Grace", "Heidi", "Ivan", "Judy", "Liam", "Olivia", "Noah", "Emma", "Lucas", "Ava", "Mia", "Leo", "Sophie", "Ruby" };
	private static readonly string[] LastNames = { "Smith", "Jones", "Williams", "Brown", "Davies", "Wilson", "Evans", "Thomas", "Roberts", "Walker", "White", "Harris", "Clark", "Lewis", "Robinson", "Wright", "King", "Green", "Baker", "Adams" };
	private static readonly string[] StreetTypes = { "St", "Rd", "Ave", "Blvd", "Cres", "Dr", "Ct", "Ln", "Pl", "Grn" };
	private static readonly string[] Cities = { "Sydney", "Melbourne", "Brisbane", "Perth", "Adelaide", "Gold Coast", "Canberra", "Newcastle", "Wollongong", "Hobart", "Darwin", "Cairns", "Toowoomba", "Geelong", "Ballarat", "Bendigo", "Albury", "Launceston", "Townsville", "Rockhampton" };
	private static readonly string[] States = { "NSW", "VIC", "QLD", "SA", "WA", "TAS", "NT", "ACT" };

	public static List<Client> GenerateClients(int count)
	{
		List<Client> clients = new List<Client>();

		for (int i = 1; i <= count; i++)
		{
			string firstName = GetRandom(FirstNames);
			string lastName = GetRandom(LastNames);
			string clientName = $"{firstName} {lastName}";
			bool isCompany = random.Next(2) == 0; // 50% chance of being a company

			Client client = new Client
			{
				Id = NewId.NextSequentialGuid(), // Assign a unique ID
				Name = clientName,
				CompanyName = isCompany ? $"{GetRandomCompanyName()} {GetRandom(CompanySuffixes)}" : null,
				BSB = GetRandomBSB(),
				AccountNo = GenerateRandomAccountNumber(),
				StreetNumber = random.Next(1, 200).ToString(),
				Street = $"{GetRandomStreetName()} {GetRandom(StreetTypes)}",
				City = GetRandom(Cities),
				State = GetRandom(States),
				Postcode = GenerateRandomPostcode()
			};
			clients.Add(client);
		}

		return clients;
	}

	private static T GetRandom<T>(T[] array)
	{
		return array[random.Next(array.Length)];
	}

	private static string GetRandomCompanyName()
	{
		// Simple way to create a company name from first/last names
		return $"{GetRandom(FirstNames)}{GetRandom(LastNames)}";
	}

	private static string GetRandomBSB()
	{
		// Generates a BSB in the format XXX-XXX
		return $"{random.Next(100, 999)}-{random.Next(100, 999)}";
	}

	private static string GenerateRandomAccountNumber()
	{
		// Generates a 6-9 digit account number
		return random.Next(100000, 999999999).ToString();
	}

	private static string GenerateRandomPostcode()
	{
		// Generates a 4-digit Australian-like postcode
		return random.Next(1000, 9999).ToString();
	}

	private static string GetRandomStreetName()
	{
		// Generate a simple street name
		return $"{GetRandom(FirstNames)}".Replace(" ", ""); // Remove spaces just in case
	}
}
