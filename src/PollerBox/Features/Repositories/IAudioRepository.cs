using PollerBox.Models;

namespace PollerBox.Features.Repositories;

public interface IAudioRepository : IBaseRepository
{
    Task<AudioFile[]> GetAudiosAsync();
    Task<AudioFile> GetAudioAsync(string audioId);
    Task<bool> SaveAudioAsync(AudioFile track, Stream dataStream);
    Task<bool> DeleteAudioAsync(AudioFile audio);
}
