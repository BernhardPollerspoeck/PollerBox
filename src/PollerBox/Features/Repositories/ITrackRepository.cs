using PollerBox.Models;

namespace PollerBox.Features.Repositories;
public interface ITrackRepository : IBaseRepository
{
	Task<Track> GetTrackAsync(string trackId);
	Task<Track[]> GetTracksAsync();
	Task<bool> SaveTrackAsync(Track track);
	Task<bool> DeleteTrackAsync(Track track);
	Task AddAudioFile(string trackId, string audioId);
	Task RemoveAudioFile(string trackId, string audioId);
}
