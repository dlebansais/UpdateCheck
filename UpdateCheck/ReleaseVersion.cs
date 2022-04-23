namespace UpdateCheck;

using System;
using System.Diagnostics;

/// <summary>
/// Represent the version of the release.
/// </summary>
public class ReleaseVersion
{
    #region Init
    /// <summary>
    /// Initializes a new instance of the <see cref="ReleaseVersion"/> class.
    /// </summary>
    public ReleaseVersion()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ReleaseVersion"/> class.
    /// </summary>
    /// <param name="fileVersionInfo">The version number, as a file version.</param>
    public ReleaseVersion(FileVersionInfo fileVersionInfo)
    {
        Major = fileVersionInfo.FileMajorPart;
        Minor = fileVersionInfo.FileMinorPart;
        Revision = fileVersionInfo.FileBuildPart;
        Build = fileVersionInfo.FilePrivatePart;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ReleaseVersion"/> class.
    /// </summary>
    /// <param name="releaseTag">The version number, as a github release tag.</param>
    /// <param name="parsedSuccessfully">True upon return if the version tag was parsed successfully.</param>
    public ReleaseVersion(string releaseTag, out bool parsedSuccessfully)
    {
        if (releaseTag.ToUpperInvariant().StartsWith("V", StringComparison.InvariantCulture))
            releaseTag = releaseTag.Substring(1);

        string[] Split = releaseTag.Split('.');

        Major = -1;
        Minor = -1;
        Revision = -1;
        Build = -1;

        for (int i = 0; i < Split.Length; i++)
        {
            if (!int.TryParse(Split[i], out int Value))
            {
                parsedSuccessfully = false;
                return;
            }

            switch (i)
            {
                case 0:
                    Major = Value;
                    break;
                case 1:
                    Minor = Value;
                    break;
                case 2:
                    Revision = Value;
                    break;
                case 3:
                    Build = Value;
                    break;
            }
        }

        parsedSuccessfully = true;
    }
    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets the major version.
    /// </summary>
    public int Major { get; set; }

    /// <summary>
    /// Gets or sets the minor version.
    /// </summary>
    public int Minor { get; set; }

    /// <summary>
    /// Gets or sets the revision version.
    /// </summary>
    public int Revision { get; set; }

    /// <summary>
    /// Gets or sets the build number.
    /// </summary>
    public int Build { get; set; }
    #endregion

    #region Operators
    /// <summary>
    /// Checks that two versions are equal.
    /// </summary>
    /// <param name="v1">The first version.</param>
    /// <param name="v2">The second version.</param>
    /// <returns>True if equal; otherwise, false.</returns>
    public static bool operator ==(ReleaseVersion v1, ReleaseVersion v2)
    {
        if (v1.Major != v2.Major)
            return false;

        if (v1.Minor != v2.Minor)
            return false;

        if (v1.Revision != v2.Revision)
            return false;

        if (v1.Build != v2.Build)
            return false;

        return true;
    }

    /// <summary>
    /// Checks that two versions are different.
    /// </summary>
    /// <param name="v1">The first version.</param>
    /// <param name="v2">The second version.</param>
    /// <returns>True if different; otherwise, false.</returns>
    public static bool operator !=(ReleaseVersion v1, ReleaseVersion v2)
    {
        return !(v1 == v2);
    }

    /// <summary>
    /// Checks that <paramref name="v1"/> is strictly greater than <paramref name="v2"/>.
    /// </summary>
    /// <param name="v1">The first version.</param>
    /// <param name="v2">The second version.</param>
    /// <returns>True if strictly greater; otherwise, false.</returns>
    public static bool operator >(ReleaseVersion v1, ReleaseVersion v2)
    {
        if (v1.Major > v2.Major)
            return true;

        if (v1.Major < v2.Major)
            return false;

        if (v1.Minor > v2.Minor)
            return true;

        if (v1.Minor < v2.Minor)
            return false;

        if (v1.Revision > v2.Revision)
            return true;

        if (v1.Revision < v2.Revision)
            return false;

        if (v1.Build > v2.Build)
            return true;

        return false;
    }

    /// <summary>
    /// Checks that <paramref name="v1"/> is strictly lesser than <paramref name="v2"/>.
    /// </summary>
    /// <param name="v1">The first version.</param>
    /// <param name="v2">The second version.</param>
    /// <returns>True if strictly lesser; otherwise, false.</returns>
    public static bool operator <(ReleaseVersion v1, ReleaseVersion v2)
    {
        return !(v1 == v2) && !(v1 > v2);
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        return base.Equals(obj);
    }

    /// <summary>
    /// Gets the object hash code.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    #endregion
}
