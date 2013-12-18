using System;
using Exortech.NetReflector;
using ThoughtWorks.CruiseControl.Core.Util;
using ThoughtWorks.CruiseControl.Remote;
using ThoughtWorks.CruiseControl.Core.Label;
using ThoughtWorks.CruiseControl.Core;

namespace SemverCommentLabeller
{

    /// <summary>
    /// Provides a valid System.Version label for your .NET assemblies that could be used to set the AssemblyVersionAttribute(). It increments
    /// the version number using semver's rules on every successful integration by parsing comments for known prefixes. The CruiseControl.NET change number
    /// is also used to set the 4th component of the .Net version number (since this isn't necessarily meaningful for semver). The change number/revision
    /// is provided by source control systems like Subversion.
    /// </summary>
    /// <see cref="http://www.semver.org"/>
    /// <title>Semver Comment Labeller</title>
    [ReflectorType("semverCommentLabeller")]
    public class SemverCommentLabeller
        : LabellerBase
    {
        #region public properties
        /// <summary>
        /// Major number component of the version. Incremented when a comment is prefixed with the case-insensitive string "major:". 
        /// </summary>
        [ReflectorProperty("major", Required = false)]
        public int Major { get; set; }

        /// <summary>
        /// Minor number component of the version.  Set to 0 when Major is incremented, 
        /// otherwise incremented when a comment is prefixed with the case-insenstive string "minor:".
        /// </summary>
        [ReflectorProperty("minor", Required = false)]
        public int Minor { get; set; }

        /// <summary>
        /// Patch number component of the version. Set to 0 when Major or Minor are incremented, 
        /// otherwise incremented when a comment is prefixed with the case-insensitive string "patch:". 
        /// </summary>
        [ReflectorProperty("patch", Required = false)]
        public int Patch { get; set; }

        /// <summary>
        /// Revision number component of the version. If not specified the revision number is the LastChangeNumber, provided by some VCS (e.g.
        /// the svn revision with the Subversion task).
        /// </summary>
        [ReflectorProperty("revision", Required = false)]
        public int Revision { get; set; }

        /// <summary>
        /// .NET Version number components are UInt16 and this causes problems if the LastChangeNumber exceeds 65535. By default SemverCommentLabeller
        /// will set this value to LastChangeNumber % 10000 to keep the last 4 significant digits. Set this value to mod by something else.
        /// </summary>
        [ReflectorProperty("revisionModulus", Required = false)]
        public int RevisionModulusValue { get; set; }


        /// <summary>
        /// A format applied to the major part of the buildnumber. 
        /// </summary>
        /// <version>1.7</version>
        /// <default>0</default>
        [ReflectorProperty("majorLabelFormat", Required = false)]
        public string MajorLabelFormat { get; set; }


        /// <summary>
        /// A format applied to the minor part of the buildnumber. 
        /// </summary>
        [ReflectorProperty("minorLabelFormat", Required = false)]
        public string MinorLabelFormat { get; set; }


        /// <summary>
        /// A format applied to the patch part of the buildnumber. 
        /// </summary>
        [ReflectorProperty("patchLabelFormat", Required = false)]
        public string PatchLabelFormat { get; set; }


        /// <summary>
        /// A format applied to the revision part of the buildnumber. 
        /// </summary>
        [ReflectorProperty("revisionLabelFormat", Required = false)]
        public string RevisionLabelFormat { get; set; }


        #endregion

        const string PATCH_PREFIX = "PATCH:";
        const string MINOR_PREFIX = "MINOR:";
        const string MAJOR_PREFIX = "MAJOR:";

        #region ILabeller Members

        /// <summary>
        /// Generates the specified integration result.	
        /// </summary>
        /// <param name="integrationResult">The integration result.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string Generate(IIntegrationResult integrationResult)
        {
            Version oldVersion;
            int major, minor, patch, revision;

            // try getting old version
            try
            {
                Log.Debug(string.Concat("[semverCommentLabeller] Old build label is: ", integrationResult.LastIntegration.Label));
                oldVersion = new Version(integrationResult.LastIntegration.Label);
                major = oldVersion.Major;
                minor = oldVersion.Minor;
                patch = oldVersion.Build;
            }
            catch (Exception)
            {
                oldVersion = new Version(0, 0, 0, 0);
                major = minor = patch = 0;
            }


            Log.Debug(string.Concat("[semverCommentLabeller] Old version is: ", oldVersion.ToString()));

            VersionNumberChange change = VersionNumberChange.None;
            if (integrationResult.HasModifications())
            {
                foreach (var mod in integrationResult.Modifications)
                {
                    if (String.IsNullOrWhiteSpace(mod.Comment))
                    {
                        continue;
                    }
                    var comment = mod.Comment.ToUpperInvariant();

                    if (comment.StartsWith(MAJOR_PREFIX))
                    {
                        change = VersionNumberChange.Major;
                        break;
                    }
                    else if (comment.StartsWith(MINOR_PREFIX))
                    {
                        change = VersionNumberChange.Minor;
                    }
                    else if (comment.StartsWith(PATCH_PREFIX) && change == VersionNumberChange.None)
                    {
                        change = VersionNumberChange.Patch;
                    }
                }
            }

            switch (change)
            {
                case VersionNumberChange.Major:
                    major++;
                    minor = patch = 0;
                    break;
                case VersionNumberChange.Minor:
                    minor++;
                    patch = 0;
                    break;
                case VersionNumberChange.Patch:
                    patch++;
                    break;
            }

            //Calculate revision
            if (int.TryParse(integrationResult.LastChangeNumber, out revision))
            {
                Log.Debug(
                    string.Format(System.Globalization.CultureInfo.CurrentCulture, "[semverCommentLabeller] LastChangeNumber retrieved: {0}",
                    revision));

                int modVal = RevisionModulusValue > 0 ? RevisionModulusValue : 10000;
                revision %= modVal;
            }
            else
            {
                Log.Debug("[semverCommentLabeller] LastChangeNumber of source control is '{0}', set revision number to '0'.",
                          string.IsNullOrEmpty(integrationResult.LastChangeNumber)
                              ? "N/A"
                              : integrationResult.LastChangeNumber);
            }

            // use the revision from last build,
            // because LastChangeNumber is 0 on ForceBuild or other failures
            if (revision <= 0) revision = oldVersion.Revision;


            Log.Debug(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                                    "[semverCommentLabeller] Major: {0} Minor: {1} Patch: {2} Revision: {3}", major, minor, patch, revision));

            string result = string.Concat(major.ToString(MajorLabelFormat), ".", minor.ToString(MinorLabelFormat), ".", patch.ToString(PatchLabelFormat), ".", revision.ToString(RevisionLabelFormat));

            Log.Debug(string.Concat("[semverCommentLabeller] New version is: ", result));

            // return new version string
            return result;
        }

        #endregion

        enum VersionNumberChange
        {
            None = 0,
            Patch = 1,
            Minor = 2,
            Major = 3
        }
    }

    
}