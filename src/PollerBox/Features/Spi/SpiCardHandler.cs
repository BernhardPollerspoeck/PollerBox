namespace PollerBox.Features.Spi;

public class SpiCardHandler : ISpiCardHandler
{
	public event EventHandler<byte[]>? CardPresent;
	public event EventHandler? CardRemoved;

	public void OnCardRead(byte[] nfcId)
	{
		CardPresent?.Invoke(this, nfcId);
	}

	public void OnCardRemoved()
	{
		CardRemoved?.Invoke(this, EventArgs.Empty);
	}
}
