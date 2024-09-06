using CommunityToolkit.Mvvm.ComponentModel;
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
        public ApplicationInfos _applicationInfos = new();

        public HomeViewModel(IDatabaseConnection database, ITrackingService tracking)
        {

            _databaseConnection = database;
            _trackingService = tracking;
            _databaseConnection.DeleteOldFile();

            StartTracking();
        }

        private async void StartTracking()
        {
            ApplicationInfos = await _trackingService.LoadAsync();
            await Task.Run(_trackingService.TrackActiveWindow);
            return;
        }
    }

}
