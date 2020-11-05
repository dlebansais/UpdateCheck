namespace UpdateCheck
{
    using System;

    public class DownloadResult
    {
        public static Exception NoException { get; } = new Exception();

        public DownloadResult()
        {
            Content = string.Empty;
            Exception = NoException;
        }

        public DownloadResult(string content)
        {
            Content = content;
            Exception = NoException;
        }

        public DownloadResult(Exception exception)
        {
            Content = string.Empty;
            Exception = exception;
        }

        public DownloadResult(string content, Exception exception)
        {
            Content = content;
            Exception = exception;
        }

        public string Content { get; }
        public Exception Exception { get; }
    }
}
