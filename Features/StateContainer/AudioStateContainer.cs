using PollerBox.Features.StateContainer;
using System.Diagnostics.CodeAnalysis;

namespace PollerBox.Features.SignalHandler;

internal class AudioStateContainer : IAudioStateContainer
{
	private readonly object _lock = new();
	private bool _isPlaying;
	private string? _nextAudioFilename;

	public bool IsPlaying()
	{
		lock (_lock)
		{
			return _isPlaying;
		}
	}

	public void SetIsPlaying(bool isPlaying)
	{
		lock (_lock)
		{
			_isPlaying = isPlaying;
		}
	}

	public void SetNextAudio(string filename)
	{
		lock (_lock)
		{
			_nextAudioFilename = filename;
		}
	}

	public bool TryGetNextAudio([NotNullWhen(true)] out string? next)
	{
		lock (_lock)
		{
			if (_nextAudioFilename != null)
			{
				next = _nextAudioFilename;
				_nextAudioFilename = null;
				return true;
			}
			next = null;
			return false;
		}
	}
}
