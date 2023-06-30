using IMS.WebApp.Data;
using IMS.Plugins.InMemory;
using IMS.UseCases;
using Blazored.Toast;
using IMS.Plugins.EFCoreSqlServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

//Add IMS Libraries to the container
if(builder.Environment.IsEnvironment("TESTING"))
{
    builder.Services
        .AddInMemoryRepositories();
}
else
{
    builder.Services
    .AddDatabaseContext(builder.Configuration)
    .AddEfCoreRepositories();
}

builder.Services.AddUseCases();

//Add 3rd Party Libraries to the container
builder.Services
    .AddBlazoredToast();

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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
