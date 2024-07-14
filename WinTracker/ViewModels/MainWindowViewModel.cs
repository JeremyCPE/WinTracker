using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Input;
using System.Windows.Threading;
using WinTracker.Models;
using WinTracker.Communication;

namespace WinTracker.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {


        private ObservableCollection<ApplicationInfo> applicationInfos;

        private Guid _lastUsedApp;
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

        private DateTime _lastActiveTime;
        private Dispatcher _dispatcher;
        private string _lastActiveProcess;

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainWindowViewModel()
        {
            ApplicationInfos = new ObservableCollection<ApplicationInfo>();
            _lastActiveTime = DateTime.Now;
            _dispatcher = Dispatcher.CurrentDispatcher;
            StartTracking();
        }

        private void StartTracking()
        {
            Thread activeWindowThread = new Thread(new ThreadStart(TrackActiveWindow));
            activeWindowThread.Start();
        }

        private void TrackActiveWindow()
        {

                while (true)
                {
                try
                {
                    Process? process = ProcessUtils.GetForegroundProcess();
                    if (process != null)
                    {
                        ApplicationInfo appInfo = ApplicationInfo.ConvertFrom(process);
                        var usedAppInfo = ApplicationInfos.FirstOrDefault(d => d.ProcessInfo.ProcessName == appInfo.ProcessInfo.ProcessName);
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
                    }
                    Thread.Sleep(1000);
                }
            catch (Exception ex) {
               Debug.WriteLine($"[ERR] An exception has been thrown, {ex.ToString}");

            }
            }
        }

        private void UpdateStatusOfUnusedApp()
        {
            ApplicationInfos.Where(d => d.Guid != _lastUsedApp).ToList().ForEach(a => a.Stop()); 
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}

