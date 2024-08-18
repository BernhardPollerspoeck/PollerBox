namespace PollerBox.Models;

public record Track(
    string Id,
    string Title,
    string Description,
    string Image,
    List<string> AudioIds,
    TrackSettings Settings);

public record AudioFile(
    string Id,
    string Title,
    string Filename);

public record TrackSettings(
    PlaybackMode PlaybackMode);

public enum PlaybackMode
{
    RandomNoRepeat,
    Random,
    Sequential,
}

