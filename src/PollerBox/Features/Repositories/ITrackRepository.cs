using PollerBox.Models;

namespace PollerBox.Features.Repositories;

public interface ITrackRepository
{
    Task<Track> GetTrackAsync(string trackId);
    Task<Track[]> GetTracksAsync();
    Task SaveTrackAsync(Track track);
    Task DeleteTrackAsync(string trackId);
    Task AddAudioFile(string trackId, string audioId);
    Task RemoveAudioFile(string trackId, string audioId);
}
