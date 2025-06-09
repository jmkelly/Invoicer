using Invoicer.Web;

public class DatabaseSeeder
{
	private readonly DataContext context;
	private readonly SeedDataGenerator generator;

	public DatabaseSeeder(DataContext context)
	{
		this.context = context;
	}

	public async Task SeedDataAsync()
	{
		if (context.Clients.Any())
			return;

		var clients = SeedDataGenerator.GenerateClients(200);

		context.Clients.AddRange(clients);
		await context.SaveChangesAsync();
	}
}
