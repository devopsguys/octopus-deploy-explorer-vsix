// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExplorerSettingsButtonCommand.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   A command that opens the settings dialog for the explorer tool.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Infrastructure.Commands
{
    using System;
    using System.ComponentModel.Design;
    using System.Diagnostics.CodeAnalysis;

    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Services;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Views;

    /// <summary>
    /// A command that opens the settings dialog for the explorer tool.
    /// </summary>
    internal class ExplorerSettingsButtonCommand : BaseCommand
    {
        /// <summary>
        /// The explorer settings button.
        /// </summary>
        public const int Id = 0x3002;

        /// <summary>
        /// The explorer settings button unique identifier.
        /// </summary>
        public static readonly Guid Guid = new Guid("2437b242-5e0a-4706-a430-e9e346edc584");

        /// <summary>
        /// Initialises a new instance of the <see cref="ExplorerSettingsButtonCommand" /> class.
        /// </summary>
        internal ExplorerSettingsButtonCommand() : base(new CommandID(Guid, Id))
        {
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "To Review")]
        protected override void OnExecute()
        {
            base.OnExecute();
            try
            {
                var view = new ServerSettingsView { DataContext = ViewModelLocatorService.SettingsViewModel };
                view.ShowModal();
            }
            catch (Exception)
            {
            }
        }
    }
}
