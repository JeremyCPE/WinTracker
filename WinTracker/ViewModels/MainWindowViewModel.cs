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
        }

        private void TrackActiveWindow()
        {
            while (true)
            {
                Process? process = ProcessUtils.GetForegroundProcess();
                if (process != null)
                {
                    ApplicationInfos.Add(ApplicationInfo.ConvertFrom(process));
                }
                Thread.Sleep(1000);
            }
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

