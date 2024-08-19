using Wpf.Ui;
using Wpf.Ui.Controls;

namespace WinTracker.Views
{
    /// <summary>
    /// Logique d'interaction pour Navbar.xaml
    /// </summary>
    public partial class Navbar : INavigationWindow
    {
        public ViewModels.NavbarViewModel ViewModel { get; set; }

        public Navbar(ViewModels.NavbarViewModel viewModel, INavigationService navigationService)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();

            navigationService.SetNavigationControl(RootNavigation);

        }

        public INavigationView GetNavigation() => RootNavigation;
        public void CloseWindow()
        {
            throw new NotImplementedException();
        }

        public bool Navigate(Type pageType)
        {
            return RootNavigation.Navigate(pageType);
        }

        public void SetServiceProvider(IServiceProvider serviceProvider)
        {
            RootNavigation.SetServiceProvider(serviceProvider);
        }

        public void ShowWindow()
        {
            throw new NotImplementedException();
        }

        public void SetPageService(IPageService pageService)
        {
            throw new NotImplementedException();
        }
    }

}
