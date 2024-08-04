using Iot.Device.Mfrc522;
using System.Device.Spi;
using System.Runtime.InteropServices;

namespace PollerBox.Features.Spi;

public static class WebApplicationBuilderExtensions
{
	public static WebApplicationBuilder AddSpiReader(this WebApplicationBuilder builder)
	{
		if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
		{
			return builder;
		}

		builder.Services.AddSingleton(sp => SpiDevice.Create(new SpiConnectionSettings(0, 0)
		{
			ClockFrequency = 10_000_000,
			Mode = SpiMode.Mode0,
		}));
		builder.Services.AddSingleton(sp => new MfRc522(sp.GetRequiredService<SpiDevice>(), 0));

		builder.Services.AddSingleton<SpiReader>();
		builder.Services.AddSingleton<ISpiCardHandler, SpiCardHandler>();
		builder.Services.AddHostedService(sp => sp.GetRequiredService<SpiReader>());

		return builder;
	}
}
