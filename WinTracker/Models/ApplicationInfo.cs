using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
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
        public BitmapFrame Image { get; set; }

        // Use to re get the logo
        public string FileName { get; set; }
        public ProcessInfo ProcessInfo { get; set; }

        public Category Category { get; set; }

        public State State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                OnPropertyChanged(nameof(State));
            }
        }

        public TimeSpan TimeElapsed
        {
            get
            {
                return _timeElapsed;
            }
            set
            {
                _timeElapsed = value;
                OnPropertyChanged(nameof(TimeElapsed));
            }
        }

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
            if (State is State.Stopped) return;
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
                ProcessModule? module = process.MainModule;
                if (module is null) return null;

                string name = !string.IsNullOrEmpty(module.FileVersionInfo.ProductName) ?
                    module.FileVersionInfo.ProductName : process.ProcessName;

                ApplicationInfo applicationInfo = new
                    (new((uint)process.Id, name),
                    new());

                // find a way to simplify this copy paste
                using MemoryStream strm = new();
                Icon? icon = Icon.ExtractAssociatedIcon(module.FileName);
                if (icon != null)
                {
                    applicationInfo.FileName = module.FileName;
                    icon.Save(strm);
                    applicationInfo.Image = BitmapFrame.Create(strm, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);

                }
                else
                {
                    // have a default image 
                }

                return applicationInfo;

            }
            catch (Exception ex)
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
            List<ApplicationInfoDto> appInfoDtos = new();
            applicationInfos.ForEach(appInfo => appInfoDtos.Add(ToDto(appInfo)));
            return appInfoDtos;
        }

        public static ApplicationInfoDto ToDto(ApplicationInfo applicationInfo)
            => new()
            {
                Guid = applicationInfo.Guid,
                DateOnly = applicationInfo.DateOnly,
                ProcessInfo = applicationInfo.ProcessInfo,
                CategoryDto = CategoryDto.From(applicationInfo.Category),
                TimeElapsed = applicationInfo.TimeElapsed,
                FileName = applicationInfo.FileName,
            };

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal ApplicationInfo UpdateImage(ApplicationInfo applicationInfo)
        {
            using MemoryStream strm = new();
            Icon? icon = Icon.ExtractAssociatedIcon(applicationInfo.FileName);
            if (icon is not null)
            {
                icon.Save(strm);
                applicationInfo.Image = BitmapFrame.Create(strm, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);

            }
            else
            {
                // have a default image 
            }

            return applicationInfo;
        }
    }
}
