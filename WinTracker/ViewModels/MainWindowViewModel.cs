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

        public List<string> Logger { get; set; }

        private ObservableCollection<ApplicationInfo> applicationInfos;
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
            Logger = ["[DEBUG] Start tracking..."];
            Thread activeWindowThread = new Thread(new ThreadStart(TrackActiveWindow));
            activeWindowThread.Start();
        }

        private void TrackActiveWindow()
        {
            try
            {
                while (true)
                {
                    Process? process = ProcessUtils.GetForegroundProcess();
                    if (process != null)
                    {
                        ApplicationInfo appInfo = ApplicationInfo.ConvertFrom(process);
                        var usedAppInfo = ApplicationInfos.FirstOrDefault(d => d.ProcessInfo.ProcessName == appInfo.ProcessInfo.ProcessName);
                        if (usedAppInfo is null)
                        {
                           Debug.WriteLine("[INFO] New app found !");

                            _dispatcher.Invoke(() =>
                            {
                                ApplicationInfos.Add(appInfo);
                            });
                        }
                        else
                        {
                            _dispatcher.Invoke(() =>
                            {
                               Debug.WriteLine("[DEBUG] Update of a known app!");

                                usedAppInfo.Update();
                            });

                        }
                    }
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex) {
               Debug.WriteLine($"[ERR] An exception has been thrown, {ex.ToString}");

            }
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

