using Microsoft.Extensions.Hosting;
using PollerBox.Features.SoundPlayer.SignalEmitter;

namespace PollerBox.Features.Player;
internal class AudioPlayer : BackgroundService
{
	private readonly IList<ISoundPlayerSignalEmitter> _signalEmitters;

	public AudioPlayer(IEnumerable<ISoundPlayerSignalEmitter> signalEmitters)
	{
		_signalEmitters = signalEmitters.ToList();
		foreach (var signalEmitter in _signalEmitters)
		{
			signalEmitter.EmitSignal += SignalEmitter_EmitSignal;
			signalEmitter.Disposed += SignalEmitter_Disposed;
		}
	}


	protected override Task ExecuteAsync(CancellationToken stoppingToken)
	{
		throw new NotImplementedException();
	}


	private void SignalEmitter_EmitSignal(object? sender, (SoundPlayerSignal signal, string? data) e)
	{
		if (sender is not ISoundPlayerSignalEmitter signalEmitter)
		{
			return;
		}
		// Do something with the signal
	}
	private void SignalEmitter_Disposed(object? sender, EventArgs e)
	{
		if (sender is not ISoundPlayerSignalEmitter signalEmitter)
		{
			return;
		}
		signalEmitter.EmitSignal -= SignalEmitter_EmitSignal;
		signalEmitter.Disposed -= SignalEmitter_Disposed;
		_signalEmitters.Remove(signalEmitter);
	}

}
