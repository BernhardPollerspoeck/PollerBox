namespace PollerBox.Features.SignalEmitter;

internal interface ISoundPlayerSignalEmitter : IDisposable
{
    event EventHandler<Signal>? EmitSignal;
    event EventHandler? Disposed;
}
