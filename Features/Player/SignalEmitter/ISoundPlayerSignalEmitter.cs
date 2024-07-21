namespace PollerBox.Features.SoundPlayer.SignalEmitter;

internal interface ISoundPlayerSignalEmitter
{
	event EventHandler<(SoundPlayerSignal signal, string? data)>? EmitSignal;
	event EventHandler? Disposed;
}
