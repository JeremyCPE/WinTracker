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
    public class MainWindowViewModel
    {

        public ObservableCollection<ApplicationInfo> ApplicationInfos { get; set; }
        private DateTime _lastActiveTime;
        private Dispatcher _dispatcher;
        private string _lastActiveProcess;

        public event PropertyChangedEventHandler PropertyChanged;

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

            ManagementEventWatcher startWatch = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));
            startWatch.EventArrived += new EventArrivedEventHandler(ProcessStarted);
            startWatch.Start();

            ManagementEventWatcher stopWatch = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_ProcessStopTrace"));
            stopWatch.EventArrived += new EventArrivedEventHandler(ProcessStopped);
            stopWatch.Start();
        }

        private void ProcessStarted(object sender, EventArrivedEventArgs e)
        {
            if (!uint.TryParse(e.NewEvent.Properties["ProcessID"].Value?.ToString(), out uint processId));
            if (processId is 0) return; // We Ignore PID 0 for now
            string processName = GetProcessNameById(processId);

            if(string.IsNullOrEmpty(processName)) return;

            _dispatcher.Invoke(() =>
            {
                ApplicationInfos.Add(new
                    (new(processId, processName), 
                    new()));
                OnPropertyChanged(nameof(ApplicationInfos));
            });
        }

        private void ProcessStopped(object sender, EventArrivedEventArgs e)
        {
            uint.TryParse(e.NewEvent.Properties["ProcessID"].Value?.ToString(), out uint processId);
            _dispatcher.Invoke(() =>
            {
                var app = ApplicationInfos.FirstOrDefault(a => a.ProcessInfo.ProcessId == processId);
                if (app != null)
                {
                    app.State = State.Stopped;
                    OnPropertyChanged(nameof(ApplicationInfos));
                }
            });
        }

        private void TrackActiveWindow()
        {
            while (true)
            {
                IntPtr hWnd = NativeMethods.GetForegroundWindow();
                int length = NativeMethods.GetWindowTextLength(hWnd);
                StringBuilder windowTitle = new StringBuilder(length + 1);
                NativeMethods.GetWindowText(hWnd, windowTitle, windowTitle.Capacity);

                string processName = GetProcessNameFromWindow(hWnd);

                if (!string.IsNullOrEmpty(processName))
                {
                    if (processName != _lastActiveProcess)
                    {
                        UpdateUsageTime();
                        _lastActiveProcess = processName;
                        _lastActiveTime = DateTime.Now;
                    }
                }

                Thread.Sleep(1000);
            }
        }

        private void UpdateUsageTime()
        {
            if (!string.IsNullOrEmpty(_lastActiveProcess))
            {
                _dispatcher.Invoke(() =>
                {
                    var app = ApplicationInfos.FirstOrDefault(a => a.ProcessInfo.ProcessName == _lastActiveProcess);
                    if (app != null)
                    {
                        app.TimeElapsed += DateTime.Now - _lastActiveTime; // put it in the model
                        OnPropertyChanged(nameof(ApplicationInfos));
                    }
                });
            }
        }

        private string GetProcessNameFromWindow(IntPtr hWnd)
        {
            uint pid;
            NativeMethods.GetWindowThreadProcessId(hWnd, out pid);
            return GetProcessNameById(pid);
        }

        /// <summary>
        /// Get ProcessNameById return empty string if exeception raised.
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        private static string GetProcessNameById(uint pid)
        {
            try
            {
                Process p = Process.GetProcessById((int)pid);

                return p.MainModule is null ? p.ProcessName : p.MainModule.FileVersionInfo.ProductName ?? p.ProcessName;
            }
            catch(Exception ex) 
            {
                Debug.WriteLine(ex.Message);
                return string.Empty;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}

