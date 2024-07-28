using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Threading;
using WinTracker.Communication;
using WinTracker.Database;
using WinTracker.Models;
using WinTracker.Views;

namespace WinTracker.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {

        private ObservableCollection<ApplicationInfo> applicationInfos;

        private object _currentView;

        public NavbarViewModel NavbarViewModel { get; }

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                NotifyPropertyChanged(nameof(CurrentView));
            }
        }

        private Guid _lastUsedApp;



        private readonly Dispatcher _dispatcher;

        public List<int> NotReadableList { get; set; } = [];

        public ObservableCollection<ApplicationInfo> ApplicationInfos
        {
            get { return applicationInfos; }
            set
            {
                if (value != applicationInfos)
                {
                    applicationInfos = value;
                    NotifyPropertyChanged(nameof(ApplicationInfos));
                }
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        public MainWindowViewModel()
        {
            NavbarViewModel navbarViewModel = new(this);
            CurrentView = new Home();

            List<ApplicationInfo> appInfos = JsonDatabase.Load();
            appInfos.ForEach(d => d.UpdateImage(d));
            ApplicationInfos = new ObservableCollection<ApplicationInfo>(appInfos);

            _dispatcher = Dispatcher.CurrentDispatcher;
            StartTracking();

        }

        private void StartTracking()
        {
            Thread activeWindowThread = new(new ThreadStart(TrackActiveWindow));
            activeWindowThread.Start();
        }

        public void GoToHome()
        {
            CurrentView = new Home();
        }

        public void GoToDashboard()
        {
            CurrentView = new Dashboard();
        }


        /// <summary>
        /// TODO : Move to tracking services
        /// </summary>
        private void TrackActiveWindow()
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
                    if (appInfo == null)
                    {
                        if (!NotReadableList.Contains(process.Id))
                        {
                            NotReadableList.Add(process.Id);
                        }
                        continue;
                    }
                    ApplicationInfo? usedAppInfo = ApplicationInfos.FirstOrDefault(d => d.ProcessInfo.ProcessName == appInfo.ProcessInfo.ProcessName);
                    _dispatcher.Invoke(() =>
                    {
                        if (usedAppInfo is null)
                        {
                            Debug.WriteLine("[INFO] New app found !");
                            _lastUsedApp = appInfo.Guid;
                            _dispatcher.Invoke(() =>
                            {
                                ApplicationInfos.Add(appInfo);
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

                    JsonDatabase.Save(ApplicationInfo.ToDtoList(ApplicationInfos.ToList()));

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
            return process == null || NotReadableList.Contains(process.Id);
        }

        private void UpdateStatusOfUnusedApp()
        {
            ApplicationInfos.Where(d => d.Guid != _lastUsedApp).ToList().ForEach(a => a.Stop());
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}

