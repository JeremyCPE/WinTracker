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
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Media;
using System.IO;

namespace WinTracker.Models
{
    public class ApplicationInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        private TimeSpan _timeElapsed;
        public Guid Guid { get; set; }

        public ImageSource Image { get; set; }
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

        private TimeSpan TimeLastStop { get; set; }


        public ApplicationInfo(ProcessInfo processInfo, Category category)
        {
            this.Guid = Guid.NewGuid();
            this.ProcessInfo = processInfo;
            this.Category = category;

            this.TimeElapsed = TimeSpan.Zero;
            this.TimeLastStop = DateTime.Now.TimeOfDay;
            Start();

        }

        public void Stop()
        {
           if(State is State.Stopped) return;
           State = State.Stopped;
           this.TimeLastStop = DateTime.Now.TimeOfDay;
        }

        public void Start()
        {
            State = State.Running;
        }

        internal static ApplicationInfo ConvertFrom(Process process)
        {

            ApplicationInfo applicationInfo = new
                (new ( (uint)process.Id, process.MainModule.FileVersionInfo.ProductName), 
                new());
            try
            {
                using (MemoryStream strm = new MemoryStream())
                {
                    Icon? icon = Icon.ExtractAssociatedIcon(process.MainModule.FileName);
                    if (icon != null)
                    {
                        icon.Save(strm);
                        applicationInfo.Image = BitmapFrame.Create(strm, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                    }
                    else
                    {
                        // have a default image 
                    }
                }
                
            }
            catch(Exception ex) 
            {
                Debug.WriteLine(ex.ToString());
            }
            return applicationInfo;

        }

        internal void Update()
        {
            TimeElapsed = DateTime.Now.TimeOfDay - TimeLastStop;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
