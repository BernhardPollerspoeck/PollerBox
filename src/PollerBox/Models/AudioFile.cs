namespace PollerBox.Models;

public record AudioFile(
    string Id,
    string Title,
    string Filename) : IFileContainer, IIdObject;

