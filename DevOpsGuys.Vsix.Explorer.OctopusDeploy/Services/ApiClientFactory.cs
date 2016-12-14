// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiClientFactory.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   Factory to create an <see cref="IOctopusClient" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Services
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Events;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Helpers;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Infrastructure.Commands;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Infrastructure.Events;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.UI;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.ViewModels;

    using Octopus.Client;

    /// <summary>
    /// Factory to create an <see cref="IOctopusClient"/>.
    /// </summary>
    /// <seealso cref="DevOpsGuys.Vsix.Explorer.OctopusDeploy.Services.IApiClientFactory" />
    internal class ApiClientFactory : IApiClientFactory
    {
        /// <summary>
        /// The log service.
        /// </summary>
        private readonly ILogService logService;

        /// <summary>
        /// Initialises a new instance of the <see cref="ApiClientFactory"/> class.
        /// </summary>
        /// <param name="logService">The log service.</param>
        public ApiClientFactory(ILogService logService)
        {
            this.logService = logService;
            this.logService.Trace("Exit");
        }

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <returns>
        /// An <see cref="IOctopusClient" />
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "To Review")]
        public IOctopusClient GetClient()
        {
            this.logService.Trace("Enter");

            try
            {
                var eventAggregator = OctopusDeployContainerPackage.Container.Resolve<IEventAggregator>();
                var settingsService = OctopusDeployContainerPackage.Container.Resolve<ISettingsService>();
                var commandService = OctopusDeployContainerPackage.Container.Resolve<ICommandService>();

                var settings = settingsService.GetSettings();

                if (settings.OctopusServerUrl != null && !string.IsNullOrWhiteSpace(settings.ApiKey.ToDecryptedSecureString().ToUnsecureString()))
                {
                    this.logService.Info("Has octopus server details");

                    return
                        new OctopusClientFactory().CreateClient(
                            new OctopusServerEndpoint(
                                settings.OctopusServerUrl,
                                settings.ApiKey.ToDecryptedSecureString().ToUnsecureString()));
                }

                var model = new OctopusViewModel
                {
                    ExceptionMessage = OctopusDeploy.Resources.ExceptionStartMessage + Environment.NewLine + Environment.NewLine + OctopusDeploy.Resources.ODE1001,
                    ExceptionMessageButtonText = OctopusDeploy.Resources.EditSettingsButtonText,
                    ExceptionResolutionCommand = new DelegateCommand(x => commandService.Invoke(ExplorerSettingsButtonCommand.Guid, ExplorerSettingsButtonCommand.Id))
                };

                this.logService.Warn(model.ExceptionMessage);

                eventAggregator.GetEvent<OctopusModelBuiltEvent>().Publish(model);
            }
            catch (Exception ex)
            {
                this.logService.Error(ex);
            }

            this.logService.Trace("Exit");

            return null;
        }
    }
}
