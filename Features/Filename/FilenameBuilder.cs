namespace PollerBox.Features.Filename;

internal class FilenameBuilder : IFilenameBuilder
{
    public string BuildFilename(string data)
    {
        return $"userAudio/{data.ToLower()}.mp3";
    }
}