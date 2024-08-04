namespace PollerBox.Features.Spi;

public interface ISpiCardHandler
{
	event EventHandler<byte[]>? CardPresent;
	event EventHandler? CardRemoved;

	void OnCardRemoved();
	void OnCardRead(byte[] nfcId);
}
