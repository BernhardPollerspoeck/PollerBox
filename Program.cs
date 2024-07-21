using Iot.Device.Mfrc522;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PollerBox.Features.SoundPlayer.SignalEmitter;
using PollerBox.Features.Spi;
using System.Device.Spi;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton(sp => SpiDevice.Create(new SpiConnectionSettings(0, 0)
{
	ClockFrequency = 10_000_000,
	Mode = SpiMode.Mode0,
}));
builder.Services.AddSingleton(sp => new MfRc522(sp.GetRequiredService<SpiDevice>(), 0));


builder.Services.AddSingleton<SpiReader>();
builder.Services.AddSingleton<ISoundPlayerSignalEmitter>(sp => sp.GetRequiredService<SpiReader>());
builder.Services.AddHostedService(sp => sp.GetRequiredService<SpiReader>());


var host = builder.Build();



await host.RunAsync();