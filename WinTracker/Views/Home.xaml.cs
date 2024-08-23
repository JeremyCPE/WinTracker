using Wpf.Ui.Controls;

namespace WinTracker.Views
{
    /// <summary>
    /// Logique d'interaction pour Home.xaml
    /// </summary>
    public partial class Home : INavigableView<ViewModels.MainWindowViewModel>
    {
        public ViewModels.MainWindowViewModel ViewModel { get; }
        public Home(ViewModels.MainWindowViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
    }
}
