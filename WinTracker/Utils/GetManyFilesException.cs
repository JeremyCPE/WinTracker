namespace WinTracker.Utils
{
    [Serializable]
    internal class GetManyFilesException : Exception
    {
        public GetManyFilesException()
        {
        }

        public GetManyFilesException(string? message) : base(message)
        {
        }

        public GetManyFilesException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}