// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServerSettings.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   Model to hold the server access details.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Model
{
    /// <summary>
    /// Model to hold the server access details.
    /// </summary>
    public class ServerSettings
    {
        /// <summary>
        /// Gets or sets the octopus server URL.
        /// </summary>
        /// <value>
        /// The octopus server URL.
        /// </value>
        public string OctopusServerUrl { get; set; }

        /// <summary>
        /// Gets or sets the API key.
        /// </summary>
        /// <value>
        /// The API key.
        /// </value>
        public string ApiKey { get; set; }
    }
}
