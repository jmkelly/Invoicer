using Invoicer.Web;
using Invoicer.Web.Pages.Invoices;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Configure Sqlite database
builder.Services.AddDbContext<SqliteContext>(options =>
{
	var folder = Environment.SpecialFolder.LocalApplicationData;
	var path = Environment.GetFolderPath(folder);
	var dbPath = Path.Join(path, "invoicer.db");
	options.UseSqlite($"Data Source={dbPath}");
	
	if (builder.Environment.IsDevelopment())
	{
		options.EnableSensitiveDataLogging();
	}
});

// Configure PostgreSQL database for Identity
builder.Services.AddDbContext<UserContext>(options =>
{
	options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
	
	if (builder.Environment.IsDevelopment())
	{
		options.EnableSensitiveDataLogging();
	}
});

// Configure Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options => 
{
	options.SignIn.RequireConfirmedAccount = true;
	options.Password.RequireDigit = true;
	options.Password.RequireLowercase = true;
	options.Password.RequireUppercase = true;
	options.Password.RequireNonAlphanumeric = true;
	options.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<UserContext>();

// Register repositories and services
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<DatabaseSeeder>();

var app = builder.Build();

// Seed database
using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	try
	{
		var seeder = services.GetRequiredService<DatabaseSeeder>();
		await seeder.SeedDataAsync();
	}
	catch (Exception ex)
	{
		var logger = services.GetRequiredService<ILogger<Program>>();
		logger.LogError(ex, "An error occurred during database seeding.");
		// Consider graceful shutdown or specific error handling here
	}
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
