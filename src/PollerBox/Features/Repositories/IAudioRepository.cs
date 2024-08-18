using PollerBox.Models;

namespace PollerBox.Features.Repositories;

public interface IAudioRepository
{
    Task<AudioFile> GetAudioAsync(string audioId);
    Task SaveAudioAsync(AudioFile audio);
    Task DeleteAudioAsync(string audioId);
}
