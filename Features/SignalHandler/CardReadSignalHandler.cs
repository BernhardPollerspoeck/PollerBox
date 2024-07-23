using PollerBox.Features.Audio;
using PollerBox.Features.Filename;
using PollerBox.Features.SignalEmitter;
using PollerBox.Features.StateContainer;

namespace PollerBox.Features.SignalHandler;

internal class CardReadSignalHandler(
    IAudioStateContainer audioStateContainer,
    ILogger<CardReadSignalHandler> logger,
    IFilenameBuilder filenameBuilder,
    IPlayer player)
    : ISignalHandler
{
    public async Task HandleSignal(string? data)
    {
        if (data is null)
        {
            logger.LogWarning("Data is null");
            return;
        }

        if (audioStateContainer.IsPlaying())
        {
            logger.LogInformation("Player is busy. Scheduling next audio");
            audioStateContainer.SetNextAudio(filenameBuilder.BuildFilename(data));
            return;
        }

        logger.LogInformation("Playing audio");
        audioStateContainer.SetIsPlaying(true);
        await player.Play(filenameBuilder.BuildFilename(data));
    }
}
