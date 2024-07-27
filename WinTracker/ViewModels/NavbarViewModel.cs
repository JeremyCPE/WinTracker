using System.Windows.Input;

namespace WinTracker.ViewModels
{
    public class NavbarViewModel : ICommand
    {
        private readonly MainWindowViewModel _viewModel;

        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;

        public event EventHandler? CanExecuteChanged;

        public NavbarViewModel()
        {
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

