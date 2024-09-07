namespace WinTracker.Models
{
    public class UserSettings
    {
        // fill

        public bool RunAtStart { get; private set; }

        public Theme Theme { get; set; } = Theme.Light;

        public List<string> BlackList { get; private set; }

    }

    public enum Theme
    {
        Light,
        Dark,
    }
}
