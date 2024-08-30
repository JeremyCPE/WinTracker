using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Wpf.Ui;
using Wpf.Ui.Controls;
using Wpf.Ui.Demo.Mvvm.ViewModels;
namespace WinTracker.ViewModels
{
    public partial class MainWindowViewModel : ViewModel
    {

        [ObservableProperty]
        private string _applicationTitle = string.Empty;

        private object _currentView;

        [ObservableProperty]
        private ObservableCollection<object> _navigationItems = [];

        [ObservableProperty]
        private ObservableCollection<object> _navigationFooter = [];

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = [];

        private bool _isInitialized;

        public NavbarViewModel NavbarViewModel { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainWindowViewModel(INavigationService navigationService)
        {
            if (!_isInitialized)
            {
                InitializeViewModel();
            }
        }

        private void InitializeViewModel()
        {
            ApplicationTitle = "WPF UI - MVVM Demo";

            NavigationItems =
        [
            new NavigationViewItem()
            {
                Content = "Home",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
                TargetPageType = typeof(Views.Home)
            },
            new NavigationViewItem()
            {
                Content = "Dashboard",
                Icon = new SymbolIcon { Symbol = SymbolRegular.DataHistogram24 },
                TargetPageType = typeof(Views.Dashboard)
            },
        ];

            NavigationFooter =
            [
                new NavigationViewItem()
            {
                Content = "Settings",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                TargetPageType = typeof(Views.Settings)
            },
        ];

            TrayMenuItems = [new() { Header = "Home", Tag = "tray_home" }];

            _isInitialized = true;

        }
    }


}

