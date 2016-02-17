// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingsStoreProvider.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   The settings store provider to isolate us from the shell when testing.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Services
{
    using System;

    using Microsoft.VisualStudio.Settings;
    using Microsoft.VisualStudio.Shell.Settings;

    /// <summary>
    /// The settings store provider to isolate us from the shell when testing.
    /// </summary>
    internal class SettingsStoreProvider : ISettingsStoreProvider
    {
        /// <summary>
        /// The service provider.
        /// </summary>
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Initialises a new instance of the <see cref="SettingsStoreProvider"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        internal SettingsStoreProvider(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Gets the writable settings store.
        /// </summary>
        /// <returns>
        /// The <see cref="WritableSettingsStore" />.
        /// </returns>
        public WritableSettingsStore GetWritableSettingsStore()
        {
            return new ShellSettingsManager(this.serviceProvider).GetWritableSettingsStore(SettingsScope.UserSettings);
        }
    }
}
