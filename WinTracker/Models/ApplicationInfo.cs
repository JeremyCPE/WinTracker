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
using WinTracker.Dtos;

namespace WinTracker.Models
{
    public class ApplicationInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;


        private TimeSpan _timeElapsed;

        private State _state;
        public Guid Guid { get; set; }

        public DateOnly DateOnly { get; set; }
        public ImageSource? Image { get; set; }
        public ProcessInfo ProcessInfo { get; set; }
        
        public Category Category { get; set; }

        public State State { get 
            { 
                return _state; 
            } set 
            { _state = value; 
                OnPropertyChanged(nameof(State)); 
            } }

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
            this.DateOnly = DateOnly.FromDateTime(DateTime.Now);
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

        /// <summary>
        /// Convert a Process object to an ApplicationInfo, can return null if the Process is not reachable
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        internal static ApplicationInfo? ConvertFromProcess(Process process)
        {

            try
            {
                var module = process.MainModule;
                if (module is null) return null;

                var name = !string.IsNullOrEmpty(module.FileVersionInfo.ProductName) ? 
                    module.FileVersionInfo.ProductName : process.ProcessName;

                ApplicationInfo applicationInfo = new
                    (new((uint)process.Id, name),
                    new());

                using (MemoryStream strm = new MemoryStream())
                {
                    Icon? icon = Icon.ExtractAssociatedIcon(module.FileName);
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
                return applicationInfo;
                
            }
            catch(Exception ex) 
            {
                Debug.WriteLine(ex.ToString());
                return null;
            }

        }

        internal void Update()
        {
            this.State = State.Running;
            TimeElapsed += TimeSpan.FromSeconds(1);
        }

        public static List<ApplicationInfoDto> ToDtoList(List<ApplicationInfo> applicationInfos) 
        {
            var appInfoDtos = new List<ApplicationInfoDto>();
            applicationInfos.ForEach(appInfo => appInfoDtos.Add(ToDto(appInfo)));
            return appInfoDtos;
        }

        public static ApplicationInfoDto ToDto(ApplicationInfo applicationInfo)
            => new ApplicationInfoDto
            {
                Guid = applicationInfo.Guid,
                DateOnly = applicationInfo.DateOnly,
                ProcessInfo = applicationInfo.ProcessInfo,
                CategoryDto = CategoryDto.From(applicationInfo.Category),
                TimeElapsed = applicationInfo.TimeElapsed,
            };

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal ApplicationInfo UpdateImage()
        {
            // to complete
            return this;
        }
    }
}
