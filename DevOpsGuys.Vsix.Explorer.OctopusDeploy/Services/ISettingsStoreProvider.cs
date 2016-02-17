// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISettingsStoreProvider.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   The settings store provider to isolate us from the shell when testing.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Services
{
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.VisualStudio.Settings;

    /// <summary>
    /// The settings store provider to isolate us from the shell when testing.
    /// </summary>
    public interface ISettingsStoreProvider
    {
        /// <summary>
        /// Gets the writable settings store.
        /// </summary>
        /// <returns>
        /// The <see cref="WritableSettingsStore" />.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Preferred")]
        WritableSettingsStore GetWritableSettingsStore();
    }
}
