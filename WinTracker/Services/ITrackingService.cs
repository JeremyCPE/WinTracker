using WinTracker.Models;

namespace WinTracker.Utils
{
    public interface ITrackingService
    {
        Task<List<ApplicationInfo>> LoadAsync();
        ICollection<ApplicationInfo> TrackActiveWindow(ICollection<ApplicationInfo> currentList);
    }
}