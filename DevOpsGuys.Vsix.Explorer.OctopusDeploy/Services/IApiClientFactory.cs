// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IApiClientFactory.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   Contract for the Api client factory.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Services
{
    using System.Diagnostics.CodeAnalysis;

    using Octopus.Client;

    /// <summary>
    /// Contract for the API client factory.
    /// </summary>
    public interface IApiClientFactory
    {
        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <returns>An <see cref="IOctopusClient"/></returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Preferred")]
        IOctopusClient GetClient();
    }
}
