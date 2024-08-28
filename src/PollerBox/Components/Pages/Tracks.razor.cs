using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using PollerBox.Features.Repositories;
using PollerBox.Models;

namespace PollerBox.Components.Pages;

public partial class Tracks
{
	[Inject]
	protected ITrackRepository TrackRepository { get; set; } = default!;
	[Inject]
	private NavigationManager NavigationManager { get; set; } = default!;

	private string? _title;
	private string? _description;
	private string? _imagePreview;
	private Track[] _tracks = [];
	private readonly PlaybackMode[] _playbackModes =
	[
		PlaybackMode.RandomNoRepeat,
			PlaybackMode.Random,
			PlaybackMode.Sequential,
		];
	private PlaybackMode _selectedPlaybackMode = PlaybackMode.RandomNoRepeat;
	private bool _isDeleteDialogVisible;
	private Track? _trackToDelete;

	protected bool CanSave => _title is not null && _description is not null && _imagePreview is not null;
	protected bool CanNotSave => !CanSave;

	protected override async Task OnInitializedAsync()
	{
		_tracks = [.. (await TrackRepository.GetTracksAsync()).OrderBy(a => a.Title)];
	}

	private async Task Save()
	{
		if (await TrackRepository.SaveTrackAsync(
			new Track(
				Guid.NewGuid().ToString(),
				_title!,
				_description!,
				_imagePreview!,
				[],
				new TrackSettings(
					_selectedPlaybackMode))))
		{
			_title = null;
			_description = null;
			_imagePreview = null;
			_selectedPlaybackMode = PlaybackMode.RandomNoRepeat;
			await OnInitializedAsync();
		}
	}

	private Task Edit(Track track)
	{
		var trackId = track.Id;
		NavigationManager.NavigateTo($"/trackedit/{trackId}");
		return Task.CompletedTask;
	}

	private void ShowDeleteConfirmation(Track track)
	{
		_trackToDelete = track;
		_isDeleteDialogVisible = true;
	}

	private async Task OnDeleteConfirmed(bool confirmed)
	{
		if (confirmed && _trackToDelete != null)
		{
			await Delete(_trackToDelete);
		}
		_isDeleteDialogVisible = false;
	}

	private async Task Delete(Track track)
	{
		if (await TrackRepository.DeleteTrackAsync(track))
		{
			await OnInitializedAsync();
		}
	}

	private async Task OnImageFileChanged(InputFileChangeEventArgs args)
	{
		using MemoryStream ms = new();
		var resized = await args.File.RequestImageFileAsync(args.File.ContentType, 256, 256);
		using Stream fileStream = resized.OpenReadStream();
		await fileStream.CopyToAsync(ms);

		_imagePreview = $"data:{args.File.ContentType};base64,{Convert.ToBase64String(ms.ToArray())}";
	}
}
