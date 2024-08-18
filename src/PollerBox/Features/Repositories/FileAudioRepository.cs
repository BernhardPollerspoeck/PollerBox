using PollerBox.Models;

namespace PollerBox.Features.Repositories;

public class FileAudioRepository(ILogger<FileAudioRepository> logger)
    : BaseFileRepository(logger), IAudioRepository
{
    public Task DeleteAudioAsync(string audioId)
    {
        DeleteFile($"{Constants.AUDIO_PATH}/{audioId}.json");
        return Task.CompletedTask;
    }

    public Task<AudioFile> GetAudioAsync(string audioId)
    {
        return GetFile<AudioFile>($"{Constants.AUDIO_PATH}/{audioId}.json");
    }

    public Task SaveAudioAsync(AudioFile audio)
    {
        return SaveFile($"{Constants.AUDIO_PATH}/{audio.Id}.json", audio);
    }
}
