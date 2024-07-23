using Iot.Device.Mfrc522;
using PollerBox.Features.Audio;
using PollerBox.Features.Filename;
using PollerBox.Features.SignalEmitter;
using PollerBox.Features.SignalHandler;
using PollerBox.Features.Spi;
using PollerBox.Features.StateContainer;
using System.Device.Spi;
using System.Runtime.InteropServices;

var builder = WebApplication.CreateBuilder(args);

if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
{
	builder.Services.AddSingleton(sp => SpiDevice.Create(new SpiConnectionSettings(0, 0)
	{
		ClockFrequency = 10_000_000,
		Mode = SpiMode.Mode0,
	}));
	builder.Services.AddSingleton(sp => new MfRc522(sp.GetRequiredService<SpiDevice>(), 0));

	builder.Services.AddSingleton<SpiReader>();
	builder.Services.AddSingleton<ISoundPlayerSignalEmitter>(sp => sp.GetRequiredService<SpiReader>());
	builder.Services.AddHostedService(sp => sp.GetRequiredService<SpiReader>());
}

builder.Services.AddTransient<IFilenameBuilder, FilenameBuilder>();

builder.Services.AddKeyedTransient<ISignalHandler, CardReadSignalHandler>(SoundPlayerSignal.CardRead);
builder.Services.AddKeyedTransient<ISignalHandler, CardRemovedSignalHandler>(SoundPlayerSignal.CardRemoved);
builder.Services.AddKeyedTransient<ISignalHandler, RestartSignalHandler>(SoundPlayerSignal.Restart);

builder.Services.AddSingleton<IPlayer, Mp3Player>();
builder.Services.AddSingleton<IAudioStateContainer, AudioStateContainer>();



builder.Services.AddHostedService<AudioPlayerService>();

var host = builder.Build();

host.MapPost(
	"/play/{cardId}",
	async (
		string cardId,
		[FromKeyedServices(SoundPlayerSignal.CardRead)] ISignalHandler signalHandler) =>
{
	await signalHandler.HandleSignal(cardId);
	return Results.Ok();
});


await host.RunAsync();