using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Threading;
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
        public ObservableCollection<ApplicationInfo> _applicationInfos;

        public Guid _lastUsedApp;

        private IDatabaseConnection _connection;


        public List<int> NotReadableList { get; set; } = [];

        public TrackingService(IDatabaseConnection databaseConnection)
        {
            this._connection = databaseConnection;
            this.Load();
        }

        public List<ApplicationInfo> Load()
        {
            if (_applicationInfos is null)
            {
                List<ApplicationInfo> appInfos = _connection.Load();
                appInfos.ForEach(d => d.UpdateImage(d));
                return appInfos;
            }
            return _applicationInfos.ToList();

        }

        /// <summary>
        /// Track active programs on Windows, ignore those where we don't have any info
        /// </summary>
        public void TrackActiveWindow(Dispatcher _dispatcher)
        {
            while (true)
            {
                try
                {
                    Process? process = ProcessUtils.GetForegroundProcess();
                    if (IsNullOrIgnorable(process))
                    {
                        continue;
                    }

                    ApplicationInfo? appInfo = ApplicationInfo.ConvertFromProcess(process);
                    if (appInfo is null)
                    {
                        if (!NotReadableList.Contains(process.Id))
                        {
                            NotReadableList.Add(process.Id);
                        }
                        continue;
                    }
                    ApplicationInfo? usedAppInfo = _applicationInfos.FirstOrDefault(d => d.ProcessInfo.ProcessName == appInfo.ProcessInfo.ProcessName);
                    _dispatcher.Invoke(() =>
                    {
                        if (usedAppInfo is null)
                        {
                            Debug.WriteLine("[INFO] New app found !");
                            _lastUsedApp = appInfo.Guid;
                            _dispatcher.Invoke(() =>
                            {
                                _applicationInfos.Add(appInfo);
                            });
                        }
                        else
                        {
                            Debug.WriteLine("[DEBUG] Update of a known app!");
                            _lastUsedApp = usedAppInfo.Guid;
                            usedAppInfo.Update();
                        }
                        UpdateStatusOfUnusedApp();
                    });

                    _connection.Save(ApplicationInfo.ToDtoList(_applicationInfos.ToList()));

                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[ERR] An exception has been thrown, {ex.ToString}");
                }

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
