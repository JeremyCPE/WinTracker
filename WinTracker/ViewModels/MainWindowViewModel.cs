using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Threading;
using WinTracker.Database;
using WinTracker.Models;
using WinTracker.Utils;
using WinTracker.Views;

namespace WinTracker.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
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
            get { return TrackingServices._applicationInfos; }
            set
            {
                if (value != TrackingServices._applicationInfos)
                {
                    TrackingServices._applicationInfos = value;
                    NotifyPropertyChanged(nameof(ApplicationInfos));
                }
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        public MainWindowViewModel()
        {
            NavbarViewModel = new NavbarViewModel(this);
            CurrentView = new Home();

            JsonDatabase.DeleteOldFile();

            List<ApplicationInfo> appInfos = TrackingServices.Load();
            ApplicationInfos = new ObservableCollection<ApplicationInfo>(appInfos);

            _dispatcher = Dispatcher.CurrentDispatcher;
            StartTracking();

        }

        private void StartTracking()
        {
            Thread activeWindowThread = new(() => TrackingServices.TrackActiveWindow(_dispatcher));
            activeWindowThread.Start();
        }

        #region Views
        public void GoToHome()
        {
            CurrentView = new Home();
        }

        public void GoToDashboard()
        {
            CurrentView = new Dashboard();
        }

        public void GoToSettings()
        {
            CurrentView = new Settings();
        }
        #endregion
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}

