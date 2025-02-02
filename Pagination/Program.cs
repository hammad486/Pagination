using Blazorise;
using Microsoft.EntityFrameworkCore;
using Pagination.Components;
using Pagination.Models;
using Pagination.Services;
using Pagination.Torod_Integration;
using Blazorise.Charts;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddServerSideBlazor();

//builder.Services.AddHttpClient<ITorodApiClient,TorodApiClient>();
//builder.Services.AddSingleton<ITorodApiClient,TorodApiClient>();
builder.Services.AddScoped<ProductServices>();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    }).AddBootstrapProviders()
    .AddFontAwesomeIcons();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
