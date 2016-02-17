// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseCommand.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   The base implementation of a command.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Infrastructure.Commands
{
    using System;
    using System.ComponentModel.Design;

    using Microsoft.VisualStudio.Shell;

    /// <summary>
    /// The base implementation of a command.
    /// </summary>
    internal abstract class BaseCommand : OleMenuCommand
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="BaseCommand" /> class.
        /// </summary>
        /// <param name="id">The id for the command.</param>
        protected BaseCommand(CommandID id) : base(BaseCommandExecute, id)
        {
            this.BeforeQueryStatus += BaseCommandBeforeQueryStatus;
        }

        /// <summary>
        /// Called to update the current status of the command.
        /// </summary>
        protected virtual void OnBeforeQueryStatus()
        {
            this.Enabled = true;
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected virtual void OnExecute()
        {
            // var trace = $"{this.GetType().Name}.OnExecute invoked";
        }

        /// <summary>
        /// Handles the BeforeQueryStatus event of the BaseCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private static void BaseCommandBeforeQueryStatus(object sender, EventArgs e)
        {
            var command = sender as BaseCommand;
            if (command != null)
            {
                command.OnBeforeQueryStatus();
            }
        }

        /// <summary>
        /// Handles the Execute event of the BaseCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private static void BaseCommandExecute(object sender, EventArgs e)
        {
            var command = sender as BaseCommand;
            if (command != null)
            {
                command.OnExecute();
            }
        }
    }
}
