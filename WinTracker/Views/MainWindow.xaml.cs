using Wpf.Ui;
using Wpf.Ui.Controls;

namespace WinTracker.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INavigationWindow
    {
        public ViewModels.MainWindowViewModel ViewModel { get; }

        public MainWindow(ViewModels.MainWindowViewModel viewModel, INavigationService navigationService)
        {
            InitializeComponent();

            DataContext = this;

            navigationService.SetNavigationControl(RootNavigation);
        }

        INavigationView INavigationWindow.GetNavigation() => RootNavigation;

        public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);

        public void ShowWindow() => Show();

        public void CloseWindow() => Close();

        public void SetServiceProvider(IServiceProvider serviceProvider)
        {
            RootNavigation.SetServiceProvider(serviceProvider);
        }

        public void SetPageService(IPageService pageService)
        {
            RootNavigation.SetPageService(pageService);
        }
    }
}
