using PollerBox.Components;
using PollerBox.Features.Audio;
using PollerBox.Features.Repositories;
using PollerBox.Features.Spi;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

if (Environment.GetEnvironmentVariable("POLLER_BOX_USE_SPI") == "true")
{
    builder.AddSpiReader();
}
builder.AddAudioPlayer();

builder.Services.AddTransient<IAudioRepository, FileAudioRepository>();
builder.Services.AddTransient<ITrackRepository, FileTrackRepository>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();


//TODO: volume via gpio 26 / 20
//TODO: mount files directory