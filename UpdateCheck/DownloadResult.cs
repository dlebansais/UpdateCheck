namespace UpdateCheck
{
    using System;

    /// <summary>
    /// Represents the result of a download operation.
    /// </summary>
    internal class DownloadResult
    {
        /// <summary>
        /// Gets the neutral exception value.
        /// </summary>
        public static Exception NoException { get; } = new Exception();

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadResult"/> class.
        /// </summary>
        public DownloadResult()
        {
            Content = string.Empty;
            Exception = NoException;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadResult"/> class.
        /// </summary>
        /// <param name="content">The downloaded content.</param>
        public DownloadResult(string content)
        {
            Content = content;
            Exception = NoException;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadResult"/> class.
        /// </summary>
        /// <param name="exception">The download exception.</param>
        public DownloadResult(Exception exception)
        {
            Content = string.Empty;
            Exception = exception;
        }

        /// <summary>
        /// Gets the downloaded content.
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// Gets the download exception.
        /// </summary>
        public Exception Exception { get; }
    }
}
