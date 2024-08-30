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
        private TrackingService _trackingService;

        private IDatabaseConnection _databaseConnection;

        [ObservableProperty]
        public ObservableCollection<ApplicationInfo> _applicationInfos;

        public HomeViewModel()
        {

            _databaseConnection = new JsonDatabase();

            _trackingService = new TrackingService(_databaseConnection);
            _databaseConnection.DeleteOldFile();

            Task<List<ApplicationInfo>> appInfos = _trackingService.LoadAsync();
            _applicationInfos = new ObservableCollection<ApplicationInfo>(appInfos.Result);
            StartTracking();
        }

        private async void StartTracking()
        {
            while (true)
            {
                try
                {
                    List<ApplicationInfo> list = await Task.Run(_trackingService.TrackActiveWindowAsync);
                    list.ForEach(x => { ApplicationInfos.Add(x); });
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

    }
}
