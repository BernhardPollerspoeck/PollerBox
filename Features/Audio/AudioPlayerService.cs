using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PollerBox.Features.SignalEmitter;
using PollerBox.Features.SignalHandler;
using PollerBox.Features.StateContainer;

namespace PollerBox.Features.Audio;
internal class AudioPlayerService : IHostedService
{
	private readonly IPlayer _player;
	private readonly ILogger<AudioPlayerService> _logger;
	private readonly IServiceProvider _serviceProvider;
	private readonly IAudioStateContainer _audioStateContainer;

	public AudioPlayerService(
		IEnumerable<ISoundPlayerSignalEmitter> signalEmitters,
		IPlayer player,
		ILogger<AudioPlayerService> logger,
		IServiceProvider serviceProvider,
		IAudioStateContainer audioStateContainer)
	{
		foreach (var signalEmitter in signalEmitters)
		{
			signalEmitter.EmitSignal += SignalEmitter_EmitSignal;
			signalEmitter.Disposed += SignalEmitter_Disposed;
		}
		_player = player;
		_logger = logger;
		_serviceProvider = serviceProvider;
		_audioStateContainer = audioStateContainer;
		_player.PlaybackFinished += Player_PlaybackFinished;
	}

	public Task StartAsync(CancellationToken cancellationToken)
	{
		_logger.LogInformation("Starting AudioPlayer");
		return _player.Play("onboardAudio/vanuennel.mp3");//TODO: startup audio
	}
	public Task StopAsync(CancellationToken cancellationToken)
	{
		_logger.LogInformation("Stopping AudioPlayer");
		return _player.Stop();
	}

	private void Player_PlaybackFinished(object? sender, EventArgs e)
	{
		_logger.LogInformation("Playback finished");
		_audioStateContainer.SetIsPlaying(false);
		if (_audioStateContainer.TryGetNextAudio(out var next))
		{
			_logger.LogInformation("Playing next audio");
			Task.Factory.StartNew(async () => await _player.Play(next));
		}
	}

	private void SignalEmitter_EmitSignal(object? sender, Signal signal)
	{
		var handler = _serviceProvider.GetKeyedService<ISignalHandler>(signal.PlayerSignal);
		if (handler is null)
		{
			_logger.LogError("No signal handler found");
			return;
		}
		_logger.LogInformation("Handling signal {signal}", signal.PlayerSignal);
		Task.Factory.StartNew(() => handler.HandleSignal(signal.Data));
	}
	private void SignalEmitter_Disposed(object? sender, EventArgs e)
	{
		if (sender is not ISoundPlayerSignalEmitter signalEmitter)
		{
			return;
		}
		signalEmitter.EmitSignal -= SignalEmitter_EmitSignal;
		signalEmitter.Disposed -= SignalEmitter_Disposed;
	}

}
