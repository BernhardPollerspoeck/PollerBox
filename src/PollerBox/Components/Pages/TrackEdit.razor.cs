using Microsoft.AspNetCore.Components;
using PollerBox.Features.Repositories;
using PollerBox.Models;

namespace PollerBox.Components.Pages;

public partial class TrackEdit
{
    [Parameter]
    public string TrackId { get; set; } = default!;

    private Track? _track;

    [Inject]
    protected ITrackRepository TrackRepository { get; set; } = default!;

        protected override async Task OnInitializedAsync()
    {
        _track = await TrackRepository.GetTrackAsync(TrackId);
    }


}
