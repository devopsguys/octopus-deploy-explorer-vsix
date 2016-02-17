// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewModelLocatorService.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   Locates the view models for the views.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Services
{
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.ViewModels;

    /// <summary>
    /// Locates the view models for the views.
    /// </summary>
    internal static class ViewModelLocatorService
    {
        /// <summary>
        /// Gets the explorer view model.
        /// </summary>
        /// <value>
        /// The explorer view model.
        /// </value>
        public static ExplorerViewModel ExplorerViewModel
        {
            get { return OctopusDeployContainerPackage.Container.Resolve<ExplorerViewModel>(); }
        }

        /// <summary>
        /// Gets the settings view model.
        /// </summary>
        /// <value>
        /// The settings view model.
        /// </value>
        public static ServerSettingsViewModel SettingsViewModel
        {
            get { return OctopusDeployContainerPackage.Container.Resolve<ServerSettingsViewModel>(); }
        }
    }
}
