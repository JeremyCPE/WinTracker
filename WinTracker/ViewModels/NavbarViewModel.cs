namespace WinTracker.ViewModels
{
    public class NavbarViewModel
    {
        private readonly MainWindowViewModel _viewModel;


        public event EventHandler? CanExecuteChanged;

        public NavbarViewModel(MainWindowViewModel mainWindowViewModel)
        {
            _viewModel = mainWindowViewModel;
        }

    }
}

