namespace UpdateCheck
{
    using System;
    using System.IO;
    using System.Net;
    using System.Security;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Threading;

    /// <summary>
    /// Provide tools to download from the Internet.
    /// </summary>
    internal static class WebClientTool
    {
        /// <summary>
        /// Handles the result of a download.
        /// </summary>
        /// <param name="result">The download result.</param>
        public delegate void DownloadTextResultHandler(DownloadResult result);

        /// <summary>
        /// Downloads a page of text from the Internet.
        /// </summary>
        /// <param name="address">The page address.</param>
        /// <param name="callback">The download handler.</param>
        public static void DownloadText(string address, DownloadTextResultHandler callback)
        {
            Task<DownloadResult> DownloadTask = new Task<DownloadResult>(() => { return ExecuteDownloadText(address); });
            DownloadTask.Start();

            PollDownload(DownloadTask, callback);
        }

        private static DownloadResult ExecuteDownloadText(string address)
        {
            try
            {
                HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(new Uri(address));
                using WebResponse Response = Request.GetResponse();
                using Stream ResponseStream = Response.GetResponseStream();
                Encoding Encoding = Encoding.ASCII;
                string[] ContentTypeSplit = Response.ContentType.ToUpperInvariant().Split(';');
                foreach (string s in ContentTypeSplit)
                {
                    string Chunk = s.Trim();
                    if (Chunk.StartsWith("CHARSET", StringComparison.InvariantCulture))
                    {
                        string[] ChunkSplit = Chunk.Split('=');
                        if (ChunkSplit.Length == 2)
                            if (ChunkSplit[0] == "CHARSET")
                                if (ChunkSplit[1] == "UTF8" || ChunkSplit[1] == "UTF-8")
                                {
                                    Encoding = Encoding.UTF8;
                                    break;
                                }
                    }
                }

                using StreamReader Reader = new StreamReader(ResponseStream, Encoding);
                string Content = Reader.ReadToEnd();

                bool IsReadToEnd = Reader.EndOfStream;
                if (IsReadToEnd)
                {
                    return new DownloadResult(Content);
                }
                else
                    return new DownloadResult();
            }
            catch (FormatException e)
            {
                return new DownloadResult(e);
            }
            catch (InvalidOperationException e)
            {
                return new DownloadResult(e);
            }
            catch (SecurityException e)
            {
                return new DownloadResult(e);
            }
            catch (NotSupportedException e)
            {
                return new DownloadResult(e);
            }
        }

        private static void PollDownload(Task<DownloadResult> downloadTask, DownloadTextResultHandler callback)
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action<Task<DownloadResult>, DownloadTextResultHandler>(OnCheckDownload), downloadTask, callback);
        }

        private static void OnCheckDownload(Task<DownloadResult> downloadTask, DownloadTextResultHandler callback)
        {
            if (downloadTask.IsCompleted)
            {
                DownloadResult Result = downloadTask.Result;
                callback(Result);
            }
            else
                PollDownload(downloadTask, callback);
        }
    }
}
