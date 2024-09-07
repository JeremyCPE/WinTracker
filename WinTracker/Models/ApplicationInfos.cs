using System.Collections.ObjectModel;

namespace WinTracker.Models
{
    public class ApplicationInfos
    {
        /// <summary>
        /// Cache applicationInfo
        /// </summary>
        /// 
        public ObservableCollection<ApplicationInfo> List { get; set; } = new ObservableCollection<ApplicationInfo>();
    }
}
