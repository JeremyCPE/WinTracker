using System.Windows.Input;

namespace WinTracker.ViewModels
{
    public class SettingsViewModel
    {
        public bool RunAtStartBool { get; set; }
        public int LogsLength { get; set; }
        public ICommand RunAtStartCommand { get; set; }
        public ICommand LogsLengthCommand { get; set; }

        public SettingsViewModel()
        {
            // TODO : Use other system
            //   RunAtStartCommand = new RelayCommand(SetRunAtStart);
            //   LogsLengthCommand = new RelayCommand(SetLogsLength);
        }

        public void SetRunAtStart(bool state)
        {
            // userSettings
        }

        public void SetLogsLength(int value)
        {
            // userSettings
        }
    }
}
