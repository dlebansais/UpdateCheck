namespace UpdateCheck
{
    using System;
    using System.Diagnostics;
    using System.Reflection;

    /// <summary>
    /// Checks if a GitHub project has a new release.
    /// </summary>
    public class Checker
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="Checker"/> class.
        /// The project should be at https://github.com/&lt;project_owner&gt;/&lt;project_name&gt;.
        /// </summary>
        /// <param name="projectOwner">The project owner.</param>
        /// <param name="projectName">The project name.</param>
        public Checker(string projectOwner, string projectName)
        {
            ProjectOwner = projectOwner;
            ProjectName = projectName;
            BinaryLocation = Assembly.GetCallingAssembly().Location;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the project owner.
        /// </summary>
        public string ProjectOwner { get; }

        /// <summary>
        /// Gets the project name.
        /// </summary>
        public string ProjectName { get; }

        /// <summary>
        /// Gets the binary location.
        /// </summary>
        public string BinaryLocation { get; }

        /// <summary>
        /// Gets the Release page address.
        /// </summary>
        public string ReleasePageAddress { get { return "https" + $"://github.com/{ProjectOwner}/{ProjectName}/releases"; } }

        /// <summary>
        /// Gets a value indicating whether an update is available.
        /// </summary>
        public bool? IsUpdateAvailable { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks for update. When the check is completed, <see cref="IsUpdateAvailable"/> contains the result.
        /// </summary>
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
        /// <summary>
        /// Indicates when <see cref="IsUpdateAvailable"/> has been updated.
        /// </summary>
        public event EventHandler? UpdateStatusChanged;

        /// <summary>
        /// Notifies handlers of <see cref="UpdateStatusChanged"/> that <see cref="IsUpdateAvailable"/> has been updated.
        /// </summary>
        protected void NotifyUpdateStatusChanged()
        {
            UpdateStatusChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
