using System.Collections.ObjectModel;
using System.ComponentModel;

namespace WinTracker.Models
{
    public class ApplicationInfos : INotifyPropertyChanged
    {
        /// <summary>
        /// Cache applicationInfo
        /// </summary>
        /// 
        public ObservableCollection<ApplicationInfo> _list;
        public ObservableCollection<ApplicationInfo> List
        {
            get
            {
                return _list;
            }
            set
            {
                _list = value;
                OnPropertyChanged(nameof(List));
            }
        }

        public ApplicationInfos()
        {
            _list = new ObservableCollection<ApplicationInfo>();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
