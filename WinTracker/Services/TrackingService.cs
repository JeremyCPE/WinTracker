using System.Diagnostics;
using WinTracker.Communication;
using WinTracker.Database;
using WinTracker.Models;

namespace WinTracker.Utils
{
    public class TrackingService : ITrackingService
    {

        private IDatabaseConnection _connection;

        private ApplicationInfos _applicationInfos;

        public static HashSet<int> NotReadableProcessIds { get; set; } = [];

        public TrackingService(IDatabaseConnection databaseConnection)
        {
            this._connection = databaseConnection;
        }

        public async Task<ApplicationInfos> LoadAsync()
        {
            if (_applicationInfos is null)
            {
                List<ApplicationInfo> appInfos = await _connection.LoadAsync();
                _applicationInfos = new();
                appInfos.ForEach(delegate (ApplicationInfo app)
                {
                    app.UpdateImage(app);
                    _applicationInfos.List.Add(app);
                }
                );
            }
            return _applicationInfos;
        }

        /// <summary>
        /// Track active programs on Windows, ignore those where we don't have any info
        /// </summary>
        public async void TrackActiveWindow()
        {
            while (true)
            {
                try
                {
                    Process? process = ProcessUtils.GetForegroundProcess();
                    if (IsNullOrIgnorable(process)) continue;

                    ApplicationInfo? appInfo = ApplicationInfo.FromProcess(process);
                    if (appInfo is null)
                    {
                        NotReadableProcessIds.Add(process.Id);
                        continue;
                    }

                    ApplicationInfo? usedAppInfo = _applicationInfos.List.FirstOrDefault(d => d.ProcessInfo.ProcessName == appInfo.ProcessInfo.ProcessName);
                    Guid lastUsedApp = appInfo.Guid;

                    if (usedAppInfo is null)
                    {
                        Debug.WriteLine("[INFO] New app found !");
                        App.Current.Dispatcher.Invoke((Action)delegate
                        {
                            _applicationInfos.List.Add(appInfo);
                        });
                    }
                    else
                    {
                        Debug.WriteLine($"[DEBUG] {DateTime.Now} Update of a known app!");
                        lastUsedApp = usedAppInfo.Guid;
                        usedAppInfo.Update();
                    }
                    UpdateStatusOfUnusedApp(_applicationInfos.List.ToList(), lastUsedApp);

                    _connection.SaveAsync(ApplicationInfo.ToDtoList(_applicationInfos.List.ToList()));
                    await Task.Delay(1000);
                    continue;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[ERR] An exception has been thrown, {ex.Message}");
                    continue;
                }
            }

        }

        private bool IsNullOrIgnorable(Process? process)
        {
            return process is null || NotReadableProcessIds.Contains(process.Id);
        }

        private void UpdateStatusOfUnusedApp(List<ApplicationInfo> currentList, Guid lastUsedApp)
        {
            currentList.Where(d => d.Guid != lastUsedApp && d.State == State.Running).ToList().ForEach(a => a.Stop());
        }

    }
}
