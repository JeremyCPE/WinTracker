using System.ComponentModel;

namespace WinTracker.ViewModels
{
    public class NavbarViewModel : INotifyPropertyChanged
    {
        private MainWindowViewModel _mainWindowViewModel;

        public event PropertyChangedEventHandler? PropertyChanged;


        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            /*
            if (parameter.ToString() == "Home")
            {
                _mainWindowViewModel.SelectedViewModel = new HomeViewModel();
            }
            else if (parameter.ToString() == "Account")
            {
                _mainWindowViewModel.SelectedViewModel = new AccountViewModel();
            }
            */
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

