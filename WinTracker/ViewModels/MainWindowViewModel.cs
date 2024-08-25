using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Threading;
using WinTracker.Database;
using WinTracker.Models;
using WinTracker.Utils;
using Wpf.Ui.Demo.Mvvm.ViewModels;
namespace WinTracker.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {

        private object _currentView;

        private TrackingService _trackingService;

        private IDatabaseConnection _databaseConnection;

        public NavbarViewModel NavbarViewModel { get; }

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                NotifyPropertyChanged(nameof(CurrentView));
            }
        }

        private readonly Dispatcher _dispatcher;


        public ObservableCollection<ApplicationInfo> ApplicationInfos
        {
            get { return _trackingService._applicationInfos; }
            set
            {
                if (value != _trackingService._applicationInfos)
                {
                    _trackingService._applicationInfos = value;
                    NotifyPropertyChanged(nameof(ApplicationInfos));
                }
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        public MainWindowViewModel()
        {
            NavbarViewModel = new NavbarViewModel(this);
            //CurrentView = new Home();

            _databaseConnection = new JsonDatabase();

            _trackingService = new TrackingService(_databaseConnection);

            _databaseConnection.DeleteOldFile();

            List<ApplicationInfo> appInfos = _trackingService.Load();
            ApplicationInfos = new ObservableCollection<ApplicationInfo>(appInfos);

            _dispatcher = Dispatcher.CurrentDispatcher;
            StartTracking();

        }

        private void StartTracking()
        {
            Thread activeWindowThread = new(() => _trackingService.TrackActiveWindow(_dispatcher));
            activeWindowThread.Start();
        }

        #region Views
        public void GoToHome()
        {
            //CurrentView = new Home();
        }

        public void GoToDashboard()
        {
            //CurrentView = new Dashboard();
        }

        public void GoToSettings()
        {
            //CurrentView = new Settings();
        }
        #endregion
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}

