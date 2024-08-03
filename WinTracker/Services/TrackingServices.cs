using System.Collections.ObjectModel;
using WinTracker.Database;
using WinTracker.Models;

namespace WinTracker.Utils
{
    public static class TrackingServices
    {
        /// <summary>
        /// Cache applicationInfo
        /// </summary>
        public static ObservableCollection<ApplicationInfo> _applicationInfos;

        public static List<ApplicationInfo> Load()
        {
            if (_applicationInfos is null)
            {
                List<ApplicationInfo> appInfos = JsonDatabase.Load();
                appInfos.ForEach(d => d.UpdateImage(d));
                return appInfos;
            }
            return _applicationInfos.ToList();

        }


    }
}
