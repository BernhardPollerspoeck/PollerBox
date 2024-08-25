namespace PollerBox.Models;

public record Track(
    string Id,
    string Title,
    string Description,
    string Image,
    List<string> AudioIds,
    TrackSettings Settings) : IIdObject;

public record AudioFile(
    string Id,
    string Title,
    string Filename) : IFileContainer, IIdObject;

public record TrackSettings(
    PlaybackMode PlaybackMode);

public interface IFileContainer
{
    string Filename { get; }
}
public interface IIdObject
{
    string Id { get; }
}
public enum PlaybackMode
{
    RandomNoRepeat,
    Random,
    Sequential,
}

