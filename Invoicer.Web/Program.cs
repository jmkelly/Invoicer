using Invoicer.Web;
using Invoicer.Web.Pages.Invoices;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<DataContext>(options =>
		{
			options.EnableSensitiveDataLogging();
			options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
		});

builder.Services.AddDbContext<UserContext>(options =>
		{
			options.EnableSensitiveDataLogging();
			options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
		});

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<UserContext>();

builder.Services.AddRazorPages();

builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();


var app = builder.Build();



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
