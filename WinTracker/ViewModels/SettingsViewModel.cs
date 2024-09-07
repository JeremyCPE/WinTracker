
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WinTracker.Database;
using WinTracker.Models;
using WinTracker.Utils;
using Wpf.Ui.Appearance;
using Wpf.Ui.Demo.Mvvm.ViewModels;

namespace WinTracker.ViewModels;
public partial class SettingsViewModel : ViewModel
{
    private bool _isInitialized = false;

    private IDictionary<Theme, ApplicationTheme> SettingsDictionary = new Dictionary<Theme, ApplicationTheme>();

    UserSettings _userSettings = new();

    IDatabaseConnection _databaseConnection;

    [ObservableProperty]
    private string _appVersion = string.Empty;

    [ObservableProperty]
    private Wpf.Ui.Appearance.ApplicationTheme _currentApplicationTheme = Wpf.Ui
        .Appearance
        .ApplicationTheme
        .Unknown;

    public SettingsViewModel(IDatabaseConnection databaseConnection)
    {
        this._databaseConnection = databaseConnection;
    }
    public override void OnNavigatedTo()
    {
        if (!_isInitialized)
        {
            InitializeViewModel();
        }
    }

    private void InitializeViewModel()
    {
        CurrentApplicationTheme = Wpf.Ui.Appearance.ApplicationThemeManager.GetAppTheme();

        SettingsDictionary[Theme.Light] = Wpf.Ui.Appearance.ApplicationTheme.Light;

        SettingsDictionary[Theme.Dark] = Wpf.Ui.Appearance.ApplicationTheme.Dark;

        AppVersion = $"WinTracker - {GetAssemblyVersion()}";

        _isInitialized = true;
    }

    private static string GetAssemblyVersion()
    {
        return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString()
            ?? string.Empty;
    }

    [RelayCommand]
    private void OnChangeTheme(string parameter)
    {
        switch (parameter)
        {
            case "theme_light":
                if (CurrentApplicationTheme != SettingsDictionary[Theme.Light])
                {
                    Wpf.Ui.Appearance.ApplicationThemeManager.Apply(Wpf.Ui.Appearance.ApplicationTheme.Light);
                    CurrentApplicationTheme = Wpf.Ui.Appearance.ApplicationTheme.Light;
                    _userSettings.Theme = Theme.Light;
                }
                break;
            case "theme_dark":
                if (CurrentApplicationTheme != SettingsDictionary[Theme.Dark])
                {
                    Wpf.Ui.Appearance.ApplicationThemeManager.Apply(Wpf.Ui.Appearance.ApplicationTheme.Dark);
                    CurrentApplicationTheme = Wpf.Ui.Appearance.ApplicationTheme.Dark;
                    _userSettings.Theme = Theme.Dark;
                }
                break;
            default: throw new ThemeNotImplementedException(parameter);
        }
    }

    [RelayCommand]
    private void Save()
    {
        _databaseConnection.SaveAsync(_userSettings);
    }
}
