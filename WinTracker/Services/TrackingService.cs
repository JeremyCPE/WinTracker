using System.Diagnostics;
using WinTracker.Communication;
using WinTracker.Database;
using WinTracker.Models;

namespace WinTracker.Utils
{
    public class TrackingService
    {
        /// <summary>
        /// Cache applicationInfo
        /// </summary>
        public List<ApplicationInfo> _applicationInfos = new();

        public Guid _lastUsedApp;

        private IDatabaseConnection _connection;


        public List<int> NotReadableList { get; set; } = [];

        public TrackingService(IDatabaseConnection databaseConnection)
        {
            this._connection = databaseConnection;
            Task.Run(LoadAsync);
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
        public async Task<List<ApplicationInfo>> TrackActiveWindowAsync()
        {
            try
            {
                Process? process = ProcessUtils.GetForegroundProcess();
                if (IsNullOrIgnorable(process))
                {
                    return _applicationInfos;
                }

                ApplicationInfo? appInfo = ApplicationInfo.FromProcess(process);
                if (appInfo is null)
                {
                    if (!NotReadableList.Contains(process.Id))
                    {
                        NotReadableList.Add(process.Id);
                    }
                    return _applicationInfos;
                }
                ApplicationInfo? usedAppInfo = _applicationInfos.FirstOrDefault(d => d.ProcessInfo.ProcessName == appInfo.ProcessInfo.ProcessName);
                if (usedAppInfo is null)
                {
                    Debug.WriteLine("[INFO] New app found !");
                    _lastUsedApp = appInfo.Guid;

                    _applicationInfos.Add(appInfo);
                }
                else
                {
                    Debug.WriteLine("[DEBUG] Update of a known app!");
                    _lastUsedApp = usedAppInfo.Guid;
                    usedAppInfo.Update();
                }
                UpdateStatusOfUnusedApp();

                _connection.SaveAsync(ApplicationInfo.ToDtoList(_applicationInfos.ToList()));

                return _applicationInfos;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERR] An exception has been thrown, {ex.ToString}");
                return _applicationInfos;
            }
        }

        private bool IsNullOrIgnorable(Process? process)
        {
            return process is null || NotReadableList.Contains(process.Id);
        }

        private void UpdateStatusOfUnusedApp()
        {
            _applicationInfos.Where(d => d.Guid != _lastUsedApp).ToList().ForEach(a => a.Stop());
        }

    }
}
