using WinTracker.Models;

namespace WinTracker.Utils
{
    public interface ITrackingService
    {
        /// <summary>
        /// Load the data from cache or from database (if cache unavailable)
        /// </summary>
        /// <returns></returns>
        Task<ApplicationInfos> LoadAsync();
        void TrackActiveWindow();
    }
}