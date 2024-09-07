namespace WinTracker.Utils
{
    [Serializable]
    internal class ThemeNotImplementedException : Exception
    {
        public ThemeNotImplementedException()
        {
        }

        public ThemeNotImplementedException(string? message) : base(message)
        {
        }

        public ThemeNotImplementedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}