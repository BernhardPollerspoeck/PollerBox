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
        return Environment.GetEnvironmentVariable("POLLER_BOX_PLAY_STARTUP") == "true"
            ? player.Play(Constants.STARTUP_AUDIO)
            : Task.CompletedTask;
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
        logger.LogInformation("Card present: {cardId}", BitConverter.ToString(cardId));
        player.Play($"{Constants.AUDIO_PATH}/{BitConverter.ToString(cardId)}.mp3");//TODO: from repository
    }
    private void SpiCardHandler_CardRemoved(object? sender, EventArgs e)
    {
        logger.LogInformation("Card removed");
        player.Stop();
    }
}
