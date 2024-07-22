using System.Diagnostics.CodeAnalysis;

namespace PollerBox.Features.StateContainer;

internal interface IAudioStateContainer
{
    bool IsPlaying();
    void SetIsPlaying(bool isPlaying);
    void SetNextAudio(string filename);
    bool TryGetNextAudio([NotNullWhen(returnValue: true)] out string? next);
}
