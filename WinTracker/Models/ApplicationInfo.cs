using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace WinTracker.Models
{
    public class ApplicationInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        private TimeSpan _timeElapsed;
        public Guid Guid { get; set; }

        public Icon? Icon { get; set; }
        public ProcessInfo ProcessInfo { get; set; }
        
        public Category Category { get; set; }

        public State State { get; set; }
        public TimeSpan TimeElapsed { get 
            {
                return _timeElapsed; 
            } set 
            { _timeElapsed = value; 
                OnPropertyChanged(nameof(TimeElapsed)); 
            } }

        private TimeSpan TimeCreated { get; }


        public ApplicationInfo(ProcessInfo processInfo, Category category)
        {
            this.Guid = Guid.NewGuid();
            this.ProcessInfo = processInfo;
            this.Category = category;

            this.TimeElapsed = TimeSpan.Zero;
            this.TimeCreated = DateTime.Now.TimeOfDay;
            Start();

        }

        public void Stop()
        {
           if(State is State.Stopped) return;
           State = State.Stopped;
           TimeElapsed = DateTime.Now.TimeOfDay - TimeCreated;
        }

        public void Start()
        {
            State = State.Running;
        }

        internal static ApplicationInfo ConvertFrom(Process process)
        {
            return (new
                (new ( (uint)process.Id, process.MainModule.FileVersionInfo.ProductName), 
                new()));
        }

        internal void Update()
        {
            TimeElapsed = DateTime.Now.TimeOfDay - TimeCreated;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
