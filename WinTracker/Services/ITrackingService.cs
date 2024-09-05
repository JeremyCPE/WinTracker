using WinTracker.Models;

namespace WinTracker.Utils
{
    public interface ITrackingService
    {
        Task<ApplicationInfos> LoadAsync();
        ApplicationInfos TrackActiveWindow();
    }
}