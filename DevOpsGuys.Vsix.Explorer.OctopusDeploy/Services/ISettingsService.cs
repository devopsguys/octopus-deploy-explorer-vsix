// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISettingsService.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   Contract for the settings service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Services
{
    using System.Diagnostics.CodeAnalysis;

    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Model;

    /// <summary>
    /// Contract for the settings service.
    /// </summary>
    public interface ISettingsService
    {
        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <returns>A <see cref="ServerSettings"/> object.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Preferred")]
        ServerSettings GetSettings();

        /// <summary>
        /// Saves the settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        void SaveSettings(ServerSettings settings);
    }
}
