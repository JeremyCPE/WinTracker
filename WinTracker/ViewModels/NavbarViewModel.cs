using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace WinTracker.ViewModels
{
    public class NavbarViewModel : ICommand
    {
        private readonly MainWindowViewModel _viewModel;

        public ICommand GoToDashboardCommand { get; }
        public ICommand GoToHomeCommand { get; }
        public ICommand GoToSettingsCommand { get; }

        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;

        public event EventHandler? CanExecuteChanged;

        public NavbarViewModel(MainWindowViewModel mainWindowViewModel)
        {
            _viewModel = mainWindowViewModel;
            GoToDashboardCommand = new RelayCommand(_viewModel.GoToDashboard);
            GoToHomeCommand = new RelayCommand(_viewModel.GoToHome);
            GoToSettingsCommand = new RelayCommand(_viewModel.GoToSettings);
        }


        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            _execute(parameter);
        }
    }
}

