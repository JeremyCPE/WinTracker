using System.Windows.Input;
using WinTracker.Utils;

namespace WinTracker.ViewModels
{
    public class NavbarViewModel : ICommand
    {
        private readonly MainWindowViewModel _viewModel;

        public ICommand GoToDashboardCommand { get; }
        public ICommand GoToHomeCommand { get; }

        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;

        public event EventHandler? CanExecuteChanged;

        public NavbarViewModel(MainWindowViewModel mainWindowViewModel)
        {
            _viewModel = mainWindowViewModel;
            GoToDashboardCommand = new RelayCommand(o => _viewModel.GoToDashboard());
            GoToHomeCommand = new RelayCommand(o => _viewModel.GoToHome());
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

