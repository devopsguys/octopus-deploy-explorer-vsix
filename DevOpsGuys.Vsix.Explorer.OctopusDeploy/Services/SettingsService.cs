// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingsService.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   Implementation of the settings service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Services
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Model;

    using Newtonsoft.Json;

    /// <summary>
    /// Implementation of the settings service.
    /// </summary>
    /// <seealso cref="ISettingsService" />
    internal class SettingsService : ISettingsService
    {
        /// <summary>
        /// The settings category name.
        /// </summary>
        public const string SettingsCategoryName = "OctopusDeployExplorer";

        /// <summary>
        /// The settings property name.
        /// </summary>
        public const string SettingsPropertyName = "Settings";

        /// <summary>
        /// The log service.
        /// </summary>
        private readonly ILogService logService;

        /// <summary>
        /// The settings store provider.
        /// </summary>
        private readonly ISettingsStoreProvider settingsStoreProvider;

        /// <summary>
        /// The settings.
        /// </summary>
        private ServerSettings settings;

        /// <summary>
        /// Initialises a new instance of the <see cref="SettingsService" /> class.
        /// </summary>
        /// <param name="logService">The log service.</param>
        /// <param name="settingsStoreProvider">The settings store provider.</param>
        public SettingsService(ILogService logService, ISettingsStoreProvider settingsStoreProvider)
        {
            this.logService = logService;
            this.settingsStoreProvider = settingsStoreProvider;
        }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <returns>
        /// A <see cref="ServerSettings" /> object.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "To Review")]
        public ServerSettings GetSettings()
        {
            if (this.settings != null)
            {
                return this.settings;
            }

            try
            {
                var store = this.settingsStoreProvider.GetWritableSettingsStore();
                if (store.PropertyExists(SettingsCategoryName, SettingsPropertyName))
                {
                    this.settings = JsonConvert.DeserializeObject<ServerSettings>(store.GetString(SettingsCategoryName, SettingsPropertyName));
                }
                else 
                {
                    this.settings = new ServerSettings();
                }
            }
            catch (JsonSerializationException ex)
            {
                this.logService.Error(ex);
                this.settings = new ServerSettings();
            }
            catch (JsonReaderException ex)
            {
                this.logService.Error(ex);
                this.settings = new ServerSettings();
            }
            catch (Exception ex)
            {
                this.logService.Error(ex);
                this.settings = new ServerSettings();
            }

            return this.settings;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        /// <param name="newSettings">The settings.</param>
        public void SaveSettings(ServerSettings newSettings)
        {
            this.settings = newSettings;

            var store = this.settingsStoreProvider.GetWritableSettingsStore();
            if (!store.CollectionExists(SettingsCategoryName))
            {
                store.CreateCollection(SettingsCategoryName);
            }

            store.SetString(SettingsCategoryName, SettingsPropertyName, JsonConvert.SerializeObject(this.settings));
        }
    }
}
