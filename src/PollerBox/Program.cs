using PollerBox.Components;
using PollerBox.Features.Audio;
using PollerBox.Features.Spi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();


builder.AddSpiReader();
builder.AddAudioPlayer();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

//app.MapPost(
//	"/play/{cardId}",
//	async (
//		string cardId,
//		[FromKeyedServices(SoundPlayerSignal.CardRead)] ISignalHandler signalHandler) =>
//	{
//		await signalHandler.HandleSignal(cardId);
//		return Results.Ok();
//	});


app.Run();
//gpio 26 / 20