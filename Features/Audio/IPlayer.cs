namespace PollerBox.Features.Audio;

internal interface IPlayer
{
	bool IsPlaying { get; }

	event EventHandler? PlaybackFinished;

	Task Play(string filename);
	Task Stop();
}
