using System.Diagnostics;
using WinTracker.Communication;
using WinTracker.Database;
using WinTracker.Models;

namespace WinTracker.Utils
{
    public class TrackingService : ITrackingService
    {
        /// <summary>
        /// Cache applicationInfo
        /// </summary>
        public List<ApplicationInfo> _applicationInfos = new();

        public Guid _lastUsedApp;

        private IDatabaseConnection _connection;


        public static List<int> NotReadableList { get; set; } = [];

        public TrackingService(IDatabaseConnection databaseConnection)
        {
            this._connection = databaseConnection;
        }

        public async Task<List<ApplicationInfo>> LoadAsync()
        {
            if (_applicationInfos is null)
            {
                List<ApplicationInfo> appInfos = await _connection.LoadAsync();
                appInfos.ForEach(d => d.UpdateImage(d));
                return appInfos;
            }
            return _applicationInfos.ToList();

        }

        /// <summary>
        /// Track active programs on Windows, ignore those where we don't have any info
        /// </summary>
        public ICollection<ApplicationInfo> TrackActiveWindow(ICollection<ApplicationInfo> currentList)
        {
            try
            {
                Process? process = ProcessUtils.GetForegroundProcess();
                if (IsNullOrIgnorable(process))
                {
                    return currentList;
                }

                ApplicationInfo? appInfo = ApplicationInfo.FromProcess(process);
                if (appInfo is null)
                {
                    if (!NotReadableList.Contains(process.Id))
                    {
                        NotReadableList.Add(process.Id);
                    }
                    return currentList;
                }
                ApplicationInfo? usedAppInfo = currentList.FirstOrDefault(d => d.ProcessInfo.ProcessName == appInfo.ProcessInfo.ProcessName);
                if (usedAppInfo is null)
                {
                    Debug.WriteLine("[INFO] New app found !");
                    _lastUsedApp = appInfo.Guid;
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        currentList.Add(appInfo);
                    });
                }
                else
                {
                    Debug.WriteLine($"[DEBUG] {DateTime.Now} Update of a known app!");
                    _lastUsedApp = usedAppInfo.Guid;
                    usedAppInfo.Update();
                }
                UpdateStatusOfUnusedApp(currentList);

                _connection.SaveAsync(ApplicationInfo.ToDtoList(currentList.ToList()));

                return currentList;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERR] An exception has been thrown, {ex.Message}");
                return currentList;
            }
        }

        private bool IsNullOrIgnorable(Process? process)
        {
            return process is null || NotReadableList.Contains(process.Id);
        }

        private void UpdateStatusOfUnusedApp(ICollection<ApplicationInfo> currentList)
        {
            currentList.Where(d => d.Guid != _lastUsedApp).ToList().ForEach(a => a.Stop());
        }

    }
}
