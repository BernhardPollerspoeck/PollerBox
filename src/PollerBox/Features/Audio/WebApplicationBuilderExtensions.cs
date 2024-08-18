namespace PollerBox.Features.Audio;

public static class WebApplicationBuilderExtensions
{
	public static WebApplicationBuilder AddAudioPlayer(this WebApplicationBuilder builder)
	{
		builder.Services.AddSingleton<IPlayer, Mp3Player>();
		builder.Services.AddHostedService<AudioPlayerService>();
		return builder;
	}
}