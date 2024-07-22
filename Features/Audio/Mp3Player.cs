using NetCoreAudio;

namespace PollerBox.Features.Audio;
internal class Mp3Player : IPlayer
{
	public event EventHandler? PlaybackFinished;

	private readonly Player _player;

	public bool IsPlaying => _player.Playing;

	public Mp3Player()
	{
		_player = new Player();
		_player.PlaybackFinished += OnPlaybackFinished;
	}

	public async Task Play(string filename)
	{
		if (IsPlaying)
		{
			await Stop();
		}

		await _player.Play(filename);
	}

	public Task Stop()
	{
		return IsPlaying
			? _player.Stop()
			: Task.CompletedTask;
	}

	private void OnPlaybackFinished(object? sender, EventArgs e)
	{
		PlaybackFinished?.Invoke(this, EventArgs.Empty);
	}
}
