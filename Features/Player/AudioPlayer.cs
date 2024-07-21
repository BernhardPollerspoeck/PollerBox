using Microsoft.Extensions.Hosting;
using PollerBox.Features.SoundPlayer.SignalEmitter;

namespace PollerBox.Features.Player;
internal class AudioPlayer : BackgroundService
{
	public AudioPlayer(IEnumerable<ISoundPlayerSignalEmitter> signalEmitters)
	{
		foreach (var signalEmitter in signalEmitters)
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
	}

}
