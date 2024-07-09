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

namespace WinTracker.ViewModels
{
    public class MainWindowViewModel
    {

        public ObservableCollection<ApplicationModel> Applications { get; set; }
        private DateTime _lastActiveTime;
        private Dispatcher _dispatcher;
        private string _lastActiveProcess;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel()
        {
            Applications = new ObservableCollection<ApplicationModel>();
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
            string processId = e.NewEvent.Properties["ProcessID"].Value?.ToString() ?? throw new ArgumentException("Process Id not found");
            string processName = GetProcessNameById(uint.Parse(processId));

            _dispatcher.Invoke(() =>
            {
                Applications.Add(new(processId, processName));
                OnPropertyChanged(nameof(Applications));
            });
        }

        private void ProcessStopped(object sender, EventArrivedEventArgs e)
        {
            string processId = e.NewEvent.Properties["ProcessID"].Value?.ToString();
            _dispatcher.Invoke(() =>
            {
                var app = Applications.FirstOrDefault(a => a.ProcessId == processId);
                if (app != null)
                {
                    app.State = State.Stopped;
                    app.UsageTime = DateTime.Now.TimeOfDay - app.TimeStart;
                    OnPropertyChanged(nameof(Applications));
                }
            });
        }

        private void TrackActiveWindow()
        {
            while (true)
            {
                IntPtr hWnd = GetForegroundWindow();
                int length = GetWindowTextLength(hWnd);
                StringBuilder windowTitle = new StringBuilder(length + 1);
                GetWindowText(hWnd, windowTitle, windowTitle.Capacity);

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
                    var app = Applications.FirstOrDefault(a => a.ProcessName == _lastActiveProcess);
                    if (app != null)
                    {
                        app.UsageTime += DateTime.Now - _lastActiveTime;
                        OnPropertyChanged(nameof(Applications));
                    }
                });
            }
        }

        private string GetProcessNameFromWindow(IntPtr hWnd)
        {
            uint pid;
            GetWindowThreadProcessId(hWnd, out pid);
            return GetProcessNameById(pid);
        }

        private static string GetProcessNameById(uint pid)
        {
            Process p = Process.GetProcessById((int)pid);

            return p.MainModule is null ? p.ProcessName : p.MainModule.FileName;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}

