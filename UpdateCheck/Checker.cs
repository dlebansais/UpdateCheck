namespace UpdateCheck;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Octokit;

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

        GitHub = new GitHubClient(new ProductHeaderValue("dlebansais.UpdateCheck"));
    }

    private readonly GitHubClient GitHub;
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
    public async Task CheckUpdateAsync()
    {
        try
        {
            IReadOnlyList<Release> ReleaseList = await GitHub.Repository.Release.GetAll(ProjectOwner, ProjectName).ConfigureAwait(false);
            OnCheckUpdate(ReleaseList);
        }
        catch
        {
            IsUpdateAvailable = false;
            NotifyUpdateStatusChanged();
        }
    }

    private void OnCheckUpdate(IReadOnlyList<Release> releaseList)
    {
        ReleaseVersion FileVersion = new(FileVersionInfo.GetVersionInfo(BinaryLocation));
        ReleaseVersion BestVersion = FileVersion;

        foreach (Release Item in releaseList)
        {
            bool IsParsedSuccessfully;

            ReleaseVersion Version = new(Item.TagName, out IsParsedSuccessfully);
            if (BestVersion < Version)
                BestVersion = Version;
        }

        IsUpdateAvailable = BestVersion != FileVersion;

        NotifyUpdateStatusChanged();
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
