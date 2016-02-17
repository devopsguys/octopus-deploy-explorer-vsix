// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandService.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   Implementation of the command service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Services
{
    using System;
    using System.ComponentModel.Design;

    /// <summary>
    /// Implementation of the command service.
    /// </summary>
    /// <seealso cref="DevOpsGuys.Vsix.Explorer.OctopusDeploy.Services.ICommandService" />
    internal class CommandService : ICommandService
    {
        /// <summary>
        /// The menu command service.
        /// </summary>
        private readonly IMenuCommandService menuCommandService;

        /// <summary>
        /// Initialises a new instance of the <see cref="CommandService"/> class.
        /// </summary>
        /// <param name="menuCommandService">The menu command service.</param>
        public CommandService(IMenuCommandService menuCommandService)
        {
            this.menuCommandService = menuCommandService;
        }

        /// <summary>
        /// Adds the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        public void Add(MenuCommand command)
        {
            this.menuCommandService.AddCommand(command);
        }

        /// <summary>
        /// Invokes the specified command unique identifier.
        /// </summary>
        /// <param name="commandGuid">The command unique identifier.</param>
        /// <param name="commandId">The command identifier.</param>
        public void Invoke(Guid commandGuid, int commandId)
        {
            this.menuCommandService.FindCommand(new CommandID(commandGuid, commandId)).Invoke(null);
        }
    }
}
