using CodenamesClean.Abstracts;
using CodenamesClean.Components;
using CodenamesClean.Hubs;
using CodenamesClean.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<IWordBank, WordBank>();
builder.Services.AddSingleton<IRoomStore, MemoryRoomStore>();
builder.Services.AddScoped<GameService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler("/Error", createScopeForErrors: true);
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapHub<GameHub>("/hub/game");

app.Run();
