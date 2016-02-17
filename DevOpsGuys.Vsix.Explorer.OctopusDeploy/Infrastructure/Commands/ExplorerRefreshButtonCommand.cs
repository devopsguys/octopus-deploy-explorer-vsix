// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExplorerRefreshButtonCommand.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   A command that refreshes the explorer tool.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Infrastructure.Commands
{
    using System;
    using System.ComponentModel.Design;

    /// <summary>
    /// A command that refreshes the explorer tool.
    /// </summary>
    internal class ExplorerRefreshButtonCommand : BaseCommand
    {
        /// <summary>
        /// The explorer refresh button.
        /// </summary>
        public const int Id = 0x3001;

        /// <summary>
        /// The explorer refresh button unique identifier.
        /// </summary>
        public static readonly Guid Guid = new Guid("6e26b076-d635-4a6e-82a8-ba10483f5414");

        /// <summary>
        /// Initialises a new instance of the <see cref="ExplorerRefreshButtonCommand" /> class.
        /// </summary>
        internal ExplorerRefreshButtonCommand() : base(new CommandID(Guid, Id))
        {
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();
            var explorer = OctopusDeployContainerPackage.Container.Resolve<OctopusDeployExplorerToolWindow>();
            if (explorer != null)
            {
                explorer.Refresh();
            } 
        }
    }
}
