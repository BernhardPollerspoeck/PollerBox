using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using PollerBox.Features.Repositories;
using PollerBox.Models;

namespace PollerBox.Components.Pages;

public partial class TrackEdit
{
	[Parameter]
	public string TrackId { get; set; } = default!;

	[Inject]
	private ITrackRepository TrackRepository { get; set; } = default!;

	[Inject]
	private IAudioRepository AudioRepository { get; set; } = default!;

	[Inject]
	private NavigationManager Navigation { get; set; } = default!;

	private List<AudioFile> _audioFiles = [];
	private readonly PlaybackMode[] _playbackModes =
	[
		PlaybackMode.Sequential,
		PlaybackMode.Random,
		PlaybackMode.RandomNoRepeat,
	];

	private Track? _track;

	private string? TrackTitle
	{
		get => _track?.Title;
		set => _track = _track with { Title = value };
	}
	private string? TrackDescription
	{
		get => _track?.Description;
		set => _track = _track with { Description = value };
	}
	private PlaybackMode? TrackPlaybackMode
	{
		get => _track?.PlaybackMode;
		set => _track = _track with { PlaybackMode = value.Value };
	}
	private string? TrackImagePreview
	{
		get => _track?.Image;
		set => _track = _track with { Image = value };
	}

	protected bool CanSave => _track is { Title: not null, Description: not null, Image: not null };
	protected bool CanNotSave => !CanSave;

	protected override async Task OnInitializedAsync()
	{
		await LoadTrack();
		await LoadAudioFiles();
	}

	private async Task LoadTrack()
	{
		_track = await TrackRepository.GetTrackAsync(TrackId);
	}

	private async Task LoadAudioFiles()
	{
		_audioFiles = [.. (await AudioRepository.GetAudiosAsync())];
	}

	private async Task SaveTrack()
	{
		if (_track is null)
		{
			return;
		}
		var success = await TrackRepository.SaveTrackAsync(_track);

		if (success)
		{
			Navigation.NavigateTo("/tracks");
		}
		else
		{
			// Handle save error
		}
	}

	private async Task OnImageFileChanged(InputFileChangeEventArgs args)
	{
		using MemoryStream ms = new();
		var resized = await args.File.RequestImageFileAsync(args.File.ContentType, 256, 256);
		using Stream fileStream = resized.OpenReadStream();
		await fileStream.CopyToAsync(ms);

		TrackImagePreview = $"data:{args.File.ContentType};base64,{Convert.ToBase64String(ms.ToArray())}";
	}
}
