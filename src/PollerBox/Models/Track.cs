namespace PollerBox.Models;

public record Track(
    string Id,
    string Title,
    string Description,
    string Image,
    List<string> AudioIds,
    TrackSettings Settings) : IIdObject;

