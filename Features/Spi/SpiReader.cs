using Iot.Device.Mfrc522;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PollerBox.Features.SignalEmitter;

namespace PollerBox.Features.Spi;

internal class SpiReader(ILogger<SpiReader> logger, MfRc522 mfRc522)
	: BackgroundService, ISoundPlayerSignalEmitter
{
	public event EventHandler<Signal>? EmitSignal;
	public event EventHandler? Disposed;

	private bool _cardPresentLastCheck = false;

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		await Task.Yield();
		try
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					//wait for a while
					await Task.Delay(1000, stoppingToken);

					//check if card is present
					var atqa = new byte[2];
					var isPresent = mfRc522.IsCardPresent(atqa);
					if (!isPresent)
					{
						if (_cardPresentLastCheck)
						{
							EmitSignal?.Invoke(this, new(SoundPlayerSignal.CardRemoved, null));
							_cardPresentLastCheck = false;
						}
						continue;
					}
					logger.LogInformation("Card is present");

					if (_cardPresentLastCheck)//TODO: doesnt work yet
					{
						// Card is still present, no need to read again
						continue;
					}

					//if card is presented, read the card
					var couldRead = mfRc522.ListenToCardIso14443TypeA(out var card, TimeSpan.FromMilliseconds(500));
					if (!couldRead)
					{
						continue;
					}
					logger.LogInformation("Card read {id}", BitConverter.ToString(card.NfcId));

					//notify service over new card read
					EmitSignal?.Invoke(this, new(SoundPlayerSignal.CardRead, BitConverter.ToString(card.NfcId)));
					_cardPresentLastCheck = true;

				}
				catch (TaskCanceledException)
				{
					//ignore. the program is shutting down
				}
				catch (Exception ex)
				{
					logger.LogError(ex, "Error in service Loop");
				}
			}
		}
		finally
		{
			Disposed?.Invoke(this, EventArgs.Empty);
			mfRc522.Dispose();
		}
	}
}
