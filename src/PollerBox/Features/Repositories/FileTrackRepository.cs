using PollerBox.Models;

namespace PollerBox.Features.Repositories;

public class FileTrackRepository(ILogger<FileTrackRepository> logger)
    : BaseFileRepository(logger), ITrackRepository
{
    public async Task AddAudioFile(string trackId, string audioId)
    {
        var track = await GetFile<Track>($"{Constants.TRACKS_PATH}/{trackId}.json");
        track.AudioIds.Add(audioId);
        await SaveTrackAsync(track);
    }

    public Task DeleteTrackAsync(string trackId)
    {
        DeleteFile($"{Constants.TRACKS_PATH}/{trackId}.json");
        return Task.CompletedTask;
    }

    public Task<Track> GetTrackAsync(string trackId)
    {
        return GetFile<Track>($"{Constants.TRACKS_PATH}/{trackId}.json");
    }

    public Task<Track[]> GetTracksAsync()
    {
        var files = GetFiles(Constants.TRACKS_PATH);
        var tracks = files.Select(GetFile<Track>);
        return Task.WhenAll(tracks);
    }

    public async Task RemoveAudioFile(string trackId, string audioId)
    {
        var track = await GetFile<Track>($"{Constants.TRACKS_PATH}/{trackId}.json");
        track.AudioIds.Remove(audioId);
        await SaveTrackAsync(track);
    }

    public Task SaveTrackAsync(Track track)
    {
        return SaveFile($"{Constants.TRACKS_PATH}/{track.Id}.json", track);
    }
}