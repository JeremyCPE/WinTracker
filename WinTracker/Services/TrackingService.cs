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

        private IDatabaseConnection _connection;

        public static HashSet<int> NotReadableProcessIds { get; set; } = [];

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
                _applicationInfos = appInfos;
                return appInfos;
            }
            return _applicationInfos.ToList();

        }

        /// <summary>
        /// Track active programs on Windows, ignore those where we don't have any info
        /// </summary>
        public ICollection<ApplicationInfo> TrackActiveWindow()
        {
            try
            {
                Process? process = ProcessUtils.GetForegroundProcess();
                if (IsNullOrIgnorable(process)) return _applicationInfos;

                ApplicationInfo? appInfo = ApplicationInfo.FromProcess(process);
                if (appInfo is null)
                {
                    NotReadableProcessIds.Add(process.Id);
                    return _applicationInfos;
                }

                ApplicationInfo? usedAppInfo = _applicationInfos.FirstOrDefault(d => d.ProcessInfo.ProcessName == appInfo.ProcessInfo.ProcessName);
                Guid lastUsedApp = appInfo.Guid;

                if (usedAppInfo is null)
                {
                    Debug.WriteLine("[INFO] New app found !");
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _applicationInfos.Add(appInfo);
                    });
                }
                else
                {
                    Debug.WriteLine($"[DEBUG] {DateTime.Now} Update of a known app!");
                    lastUsedApp = usedAppInfo.Guid;
                    usedAppInfo.Update();
                }
                UpdateStatusOfUnusedApp(_applicationInfos, lastUsedApp);

                _connection.SaveAsync(ApplicationInfo.ToDtoList(_applicationInfos.ToList()));

                return _applicationInfos;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERR] An exception has been thrown, {ex.Message}");
                return _applicationInfos;
            }
        }

        private bool IsNullOrIgnorable(Process? process)
        {
            return process is null || NotReadableProcessIds.Contains(process.Id);
        }

        private void UpdateStatusOfUnusedApp(ICollection<ApplicationInfo> currentList, Guid lastUsedApp)
        {
            currentList.Where(d => d.Guid != lastUsedApp && d.State == State.Running).ToList().ForEach(a => a.Stop());
        }

    }
}
