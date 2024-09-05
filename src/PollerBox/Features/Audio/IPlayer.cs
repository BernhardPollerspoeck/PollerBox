namespace PollerBox.Features.Audio;

public interface IPlayer
{
	bool IsPlaying { get; }

	event EventHandler? PlaybackFinished;

	Task Play(string filename);
	Task Stop();
}
