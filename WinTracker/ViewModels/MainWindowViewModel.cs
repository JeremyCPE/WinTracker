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
            get { return TrackingService._applicationInfos; }
            set
            {
                if (value != TrackingService._applicationInfos)
                {
                    TrackingService._applicationInfos = value;
                    NotifyPropertyChanged(nameof(ApplicationInfos));
                }
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        public MainWindowViewModel()
        {
            NavbarViewModel = new NavbarViewModel(this);
            //CurrentView = new Home();

            JsonDatabase.DeleteOldFile();

            List<ApplicationInfo> appInfos = TrackingService.Load();
            ApplicationInfos = new ObservableCollection<ApplicationInfo>(appInfos);

            _dispatcher = Dispatcher.CurrentDispatcher;
            StartTracking();

        }

        private void StartTracking()
        {
            Thread activeWindowThread = new(() => TrackingService.TrackActiveWindow(_dispatcher));
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

