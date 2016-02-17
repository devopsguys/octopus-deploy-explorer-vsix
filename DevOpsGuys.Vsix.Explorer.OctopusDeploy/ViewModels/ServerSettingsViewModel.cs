// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServerSettingsViewModel.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   ViewModel used to bind the server information.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.ViewModels
{
    using System;
    using System.ComponentModel;

    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Helpers;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Infrastructure;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Infrastructure.Commands;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Model;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Services;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.UI;

    /// <summary>
    /// ViewModel used to bind the server information.
    /// </summary>
    internal class ServerSettingsViewModel : Bindable
    {
        /// <summary>
        /// The settings service.
        /// </summary>
        private readonly ISettingsService settingsService;

        /// <summary>
        /// The command service.
        /// </summary>
        private readonly ICommandService commandService;

        /// <summary>
        /// The save command.
        /// </summary>
        private DelegateCommand saveCommand;

        /// <summary>
        /// The cancel command.
        /// </summary>
        private DelegateCommand cancelCommand;

        /// <summary>
        /// Initialises a new instance of the <see cref="ServerSettingsViewModel" /> class.
        /// </summary>
        /// <param name="settingsService">The settings service.</param>
        /// <param name="commandService">The command service.</param>
        public ServerSettingsViewModel(ISettingsService settingsService, ICommandService commandService)
        {
            Guard.ArgumentNotNull(settingsService, "settingsService");
            Guard.ArgumentNotNull(commandService, "commandService");
            this.settingsService = settingsService;
            this.commandService = commandService;
            this.OctopusServerUrl = this.settingsService.GetSettings().OctopusServerUrl;
            this.OctopusServerApiKey = this.settingsService.GetSettings().ApiKey;

            this.PropertyChanged += this.OnPagePropertyChanged;
        }

        /// <summary>
        /// Gets the save command.
        /// </summary>
        public DelegateCommand SaveCommand
        {
            get
            {
                return this.saveCommand ?? (this.saveCommand = new DelegateCommand(this.OnSaveCommandExecuted, this.OnSaveCommandCanExecute));
            }
        }

        /// <summary>
        /// Gets the cancel command.
        /// </summary>
        public DelegateCommand CancelCommand
        {
            get
            {
                return this.cancelCommand ?? (this.cancelCommand = new DelegateCommand(this.OnCancelCommandExecuted));
            }
        }

        /// <summary>
        /// Gets or sets the string representing the octopus deploy server.
        /// </summary>
        public Uri OctopusServerUrl
        {
            get { return this.GetPropertyValue<Uri>(); }
            set { this.SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the string representing the octopus deploy server API key.
        /// </summary>
        public string OctopusServerApiKey
        {
            get { return this.GetPropertyValue<string>(); }
            set { this.SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the dialog result.
        /// </summary>
        public bool? DialogResult
        {
            get { return this.GetPropertyValue<bool?>(); }
            set { this.SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether there are any changes that need to be saved.
        /// </summary>
        public bool HasChanges
        {
            get
            {
                return this.GetPropertyValue<bool>();
            }

            set
            {
                if (this.SetPropertyValue(value))
                {
                    this.SaveCommand.RaiseCanExecuteChanged();
                }
            }
        }
        
        /// <summary>
        /// Called when the <see cref="SaveCommand" /> needs to determine if it can execute.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        /// <returns>True if the command can execute, otherwise false.</returns>
        private bool OnSaveCommandCanExecute(object parameter)
        {
            return this.HasChanges;
        }

        /// <summary>
        /// Called when the <see cref="SaveCommand" /> is executed.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        private void OnSaveCommandExecuted(object parameter)
        {
            var settings = new ServerSettings { OctopusServerUrl = this.OctopusServerUrl, ApiKey = this.OctopusServerApiKey };
            this.settingsService.SaveSettings(settings);
            this.commandService.Invoke(ExplorerRefreshButtonCommand.Guid, ExplorerRefreshButtonCommand.Id);
            this.HasChanges = false;
            this.DialogResult = true;
        }

        /// <summary>
        /// Called when the <see cref="CancelCommand" /> is executed.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        private void OnCancelCommandExecuted(object parameter)
        {
            this.HasChanges = false;
            this.DialogResult = false;
        }

        /// <summary>
        /// Called when a page has raised a PropertyChanged event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">
        /// The <see cref="PropertyChangedEventArgs" /> instance containing
        /// the event data.
        /// </param>
        private void OnPagePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.HasChanges = true;
        }
    }
}
