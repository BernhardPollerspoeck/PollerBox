using PollerBox.Features.Spi;

namespace PollerBox.Features.Audio;
internal class AudioPlayerService(
	IPlayer player,
	ILogger<AudioPlayerService> logger,
	ISpiCardHandler? spiCardHandler = null) : IHostedService
{

	public Task StartAsync(CancellationToken cancellationToken)
	{
		logger.LogInformation("Starting AudioPlayer");
		if (spiCardHandler is not null)
		{
			spiCardHandler.CardPresent += SpiCardHandler_CardPresent;
			spiCardHandler.CardRemoved += SpiCardHandler_CardRemoved;
		}
		else
		{
			logger.LogWarning("No SPI card handler found");
		}
		return player.Play("InternalAudio/vanuennel.mp3");//TODO: startup audio
	}
	public Task StopAsync(CancellationToken cancellationToken)
	{
		logger.LogInformation("Stopping AudioPlayer");
		if (spiCardHandler is not null)
		{
			spiCardHandler.CardPresent -= SpiCardHandler_CardPresent;
			spiCardHandler.CardRemoved -= SpiCardHandler_CardRemoved;
		}
		return player.Stop();
	}

	private void SpiCardHandler_CardPresent(object? sender, byte[] cardId)
	{
		logger.LogInformation("Card present: {cardId}", cardId);
		player.Play($"audio/{BitConverter.ToString(cardId)}.mp3");
	}
	private void SpiCardHandler_CardRemoved(object? sender, EventArgs e)
	{
		logger.LogInformation("Card removed");
		player.Stop();
	}
}

public static class WebApplicationBuilderExtensions
{
	public static WebApplicationBuilder AddAudioPlayer(this WebApplicationBuilder builder)
	{
		builder.Services.AddSingleton<IPlayer, Mp3Player>();
		builder.Services.AddHostedService<AudioPlayerService>();
		return builder;
	}
}