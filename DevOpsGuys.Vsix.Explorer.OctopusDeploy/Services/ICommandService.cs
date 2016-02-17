// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICommandService.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   Wrapper around the menu command service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Services
{
    using System;
    using System.ComponentModel.Design;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Wrapper around the menu command service.
    /// </summary>
    public interface ICommandService
    {
        /// <summary>
        /// Adds the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        void Add(MenuCommand command);

        /// <summary>
        /// Invokes the specified command unique identifier.
        /// </summary>
        /// <param name="commandGuid">The command unique identifier.</param>
        /// <param name="commandId">The command identifier.</param>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "guid", Justification = "Preferred name")]
        void Invoke(Guid commandGuid, int commandId);
    }
}
