using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using WinTracker.Database;
using WinTracker.Models;
using WinTracker.Utils;
using Wpf.Ui.Demo.Mvvm.ViewModels;

namespace WinTracker.ViewModels
{
    public partial class HomeViewModel : ViewModel
    {
        private ITrackingService _trackingService;

        private IDatabaseConnection _databaseConnection;

        [ObservableProperty]
        public List<ApplicationInfo> _applicationInfos;

        public HomeViewModel(IDatabaseConnection database, ITrackingService tracking)
        {

            _databaseConnection = database;
            _trackingService = tracking;

            _trackingService = new TrackingService(_databaseConnection);
            _databaseConnection.DeleteOldFile();

            Task<List<ApplicationInfo>> appInfos = _trackingService.LoadAsync();
            _applicationInfos = new ObservableCollection<ApplicationInfo>();
            StartTracking();
        }

        private async void StartTracking()
        {
            while (true)
            {
                try
                {
                    ApplicationInfos = await Task.Run(_trackingService.TrackActiveWindow);
                    await Task.Delay(1000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

    }
}
