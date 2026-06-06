using GLMS.Data;
using GLMS.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add MVC support so controllers and views can work
builder.Services.AddControllersWithViews();

// Register the database context with SQL Server
// The connection string is pulled from appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register HttpClient now because we will need it later for the currency API
builder.Services.AddHttpClient();

// Register our service classes so they can be injected later
builder.Services.AddScoped<CurrencyService>();
builder.Services.AddScoped<FileService>();
builder.Services.AddScoped<ContractValidationService>();

builder.Services.AddHttpClient("GLMSApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]);
});

var app = builder.Build();

// This runs when the app is not in development mode
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Redirect HTTP requests to HTTPS
app.UseHttpsRedirection();

// Allow static files like CSS, JS, and uploaded files to be served
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Set the default route for the application
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();