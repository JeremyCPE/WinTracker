using System.Windows.Input;
using WinTracker.Utils;

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

            RunAtStartCommand = new RelayCommand(d => SetRunAtStart(RunAtStartBool));
            LogsLengthCommand = new RelayCommand(d => SetLogsLength(LogsLength));
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
