using PollerBox.Models;

namespace PollerBox.Features.Repositories;

public class FileAudioRepository(ILogger<FileAudioRepository> logger)
	: BaseFileRepository(logger), IAudioRepository
{
	public Task<bool> DeleteAudioAsync(AudioFile audio)
	{
		return Task.FromResult(DeleteFile(Constants.AUDIO_PATH, audio));
	}

	public Task<AudioFile> GetAudioAsync(string audioId)
	{
		return GetFile<AudioFile>($"{Constants.AUDIO_PATH}/{audioId}.json");
	}

	public Task<AudioFile[]> GetAudiosAsync()
	{
		var files = GetFiles(Constants.AUDIO_PATH);
		var tracks = files.Select(GetFile<AudioFile>);
		return Task.WhenAll(tracks);
	}

	public Task<bool> SaveAudioAsync(AudioFile audio)
	{
		return SaveFile(Constants.AUDIO_PATH, audio);
	}
	public Task<bool> SaveAudioAsync(AudioFile audio, Stream dataStream)
	{
		return SaveFile(Constants.AUDIO_PATH, audio, dataStream);
	}
	public new bool FileExists(string fileName)
	{
		return base.FileExists($"{Constants.AUDIO_PATH}/{fileName}");
	}
}
