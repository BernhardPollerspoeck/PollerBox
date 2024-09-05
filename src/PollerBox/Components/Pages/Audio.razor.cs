using Iot.Device.SocketCan;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using PollerBox.Features.Audio;
using PollerBox.Features.Repositories;
using PollerBox.Models;

namespace PollerBox.Components.Pages;

public partial class Audio
{

	[Inject]
	protected IAudioRepository AudioRepository { get; set; } = default!;

	[Inject]
	protected IPlayer Player { get; set; } = default!;

	private string? _error;
	private string? _title;
	private IBrowserFile? _selectedFile;
	private string? _selectedFileName => _selectedFile?.Name;

	private AudioFile[] _audioFiles = [];
	private AudioFile? _playingFile;
	private bool _canUpload => _selectedFile is not null && _title is not null;
	private bool _canNotUpload => !_canUpload;

	protected override async Task OnInitializedAsync()
	{
		_audioFiles = [.. (await AudioRepository.GetAudiosAsync()).OrderBy(a => a.Title)];
	}

	private void OnInputFileChange(InputFileChangeEventArgs args)
	{
		if (AudioRepository.FileExists(args.File.Name))
		{
			_error = "File already exists";
			return;
		}
		_error = null;
		_selectedFile = args.File;
	}
	private async Task Delete(AudioFile audio)
	{
		if (await AudioRepository.DeleteAudioAsync(audio))
		{
			await OnInitializedAsync();
		}
	}
	private async Task Save()
	{
		if (await AudioRepository.SaveAudioAsync(
			new AudioFile(
				Guid.NewGuid().ToString(),
				_title!,
				_selectedFile!.Name),
			_selectedFile!.OpenReadStream()))
		{
			_selectedFile = null;
			_title = null;
			await OnInitializedAsync();
		}
	}
	private async Task Play(AudioFile audio)
	{
		await Player.Stop();
		Player.PlaybackFinished += HandlePlayFinished;
		_playingFile = audio;
		await Player.Play($"{AppDomain.CurrentDomain.BaseDirectory}/{Constants.AUDIO_PATH}/{audio.Filename}");
	}

	private void HandlePlayFinished(object? sender, EventArgs args)
	{
		if (sender is IPlayer player)
		{
			player.PlaybackFinished -= HandlePlayFinished;
		}
		_playingFile = null;
		InvokeAsync(StateHasChanged);
	}
	private string GetBtnClass(AudioFile audioFile)
	{
		return audioFile == _playingFile
			? "btn-info"
			: "btn-secondary";
	}
}
