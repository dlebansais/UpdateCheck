namespace UpdateCheck
{
    using System.Reflection;
    using System.Diagnostics;
    using System;

    public class Checker
    {
        #region Init
        public Checker(string projectOwner, string projectName)
        {
            ProjectOwner = projectOwner;
            ProjectName = projectName;
            BinaryLocation = Assembly.GetCallingAssembly().Location;
        }
        #endregion

        #region Properties
        public string ProjectOwner { get; }
        public string ProjectName { get; }
        public string BinaryLocation { get; }
        public string ReleasePageAddress { get { return "https" + $"://github.com/{ProjectOwner}/{ProjectName}/releases"; } }
        public bool? IsUpdateAvailable { get; private set; }
        #endregion

        #region Client Interface
        public void CheckUpdate()
        {
            NetTools.EnableSecurityProtocol(out object OldSecurityProtocol);
            WebClientTool.DownloadText(ReleasePageAddress, (DownloadResult downloadResult) => OnCheckUpdate(downloadResult, OldSecurityProtocol));
        }

        private void OnCheckUpdate(DownloadResult downloadResult, object oldSecurityProtocol)
        {
            NetTools.RestoreSecurityProtocol(oldSecurityProtocol);
            OnCheckUpdate(downloadResult);
        }

        private void OnCheckUpdate(DownloadResult downloadResult)
        {
            if (downloadResult.Exception == DownloadResult.NoException)
                if (downloadResult.Content.Length > 0)
                    if (BinaryLocation.Length > 0)
                        OnCheckUpdate(downloadResult.Content);
        }

        private void OnCheckUpdate(string content)
        {
            string Pattern = $@"<a href=""/{ProjectOwner}/{ProjectName}/releases/tag/";
            int Index = content.IndexOf(Pattern, StringComparison.InvariantCulture);
            if (Index >= 0)
            {
                string ParserTagVersion = content.Substring(Index + Pattern.Length, 20);
                int EndIndex = ParserTagVersion.IndexOf('"');
                if (EndIndex > 0)
                {
                    ParserTagVersion = ParserTagVersion.Substring(0, EndIndex);
                    if (ParserTagVersion.ToUpperInvariant().StartsWith("V", StringComparison.InvariantCulture))
                        ParserTagVersion = ParserTagVersion.Substring(1);

                    string[] Split = ParserTagVersion.Split('.');
                    if (int.TryParse(Split[Split.Length - 1], out int buildVersion))
                    {
                        FileVersionInfo FileVersionInfo = FileVersionInfo.GetVersionInfo(BinaryLocation);
                        IsUpdateAvailable = buildVersion > FileVersionInfo.FilePrivatePart;

                        NotifyUpdateStatusChanged();
                    }
                }
            }
        }
        #endregion

        #region Events
        public event EventHandler? UpdateStatusChanged;

        protected void NotifyUpdateStatusChanged()
        {
            UpdateStatusChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
