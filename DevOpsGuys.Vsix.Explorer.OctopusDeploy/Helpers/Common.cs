// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Common.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   Common helper class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Helpers
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Common helper class.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces", Justification = "Preferred")]
    internal static class Common
    {
        /// <summary>
        /// Gets the version number.
        /// </summary>
        /// <param name="serverVersion">The server version.</param>
        /// <returns>
        /// A strongly typed <see cref="Version"/>
        /// </returns>
        public static Version GetVersionNumber(string serverVersion)
        {
            Guard.ArgumentNotNullOrEmpty(serverVersion, "serverVersion");
            var safeVersion = serverVersion.Contains("-") ? serverVersion.Substring(0, serverVersion.IndexOf("-", StringComparison.Ordinal)) : serverVersion;
            return Version.Parse(safeVersion);
        }
    }
}
