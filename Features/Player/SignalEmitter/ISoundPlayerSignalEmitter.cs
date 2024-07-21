namespace PollerBox.Features.SoundPlayer.SignalEmitter;

internal interface ISoundPlayerSignalEmitter : IDisposable
{
	event EventHandler<(SoundPlayerSignal signal, string? data)>? EmitSignal;
	event EventHandler? Disposed;
}
