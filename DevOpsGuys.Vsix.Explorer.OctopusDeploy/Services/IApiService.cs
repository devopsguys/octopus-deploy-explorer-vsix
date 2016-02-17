// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IApiService.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   Contract for the Api service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Services
{
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.ViewModels;

    /// <summary>
    /// Contract for the API service.
    /// </summary>
    internal interface IApiService
    {
        /// <summary>
        /// Gets the initial groups defined in the Octopus Deploy server.
        /// </summary>
        void GetGroups();

        /// <summary>
        /// Gets the projects defined in the group and populates the view model.
        /// </summary>
        /// <param name="group">The group.</param>
        void GetProjectsIntoGroup(OctopusGroupViewModel group);

        /// <summary>
        /// Gets the project matrix and populates the view model.
        /// </summary>
        /// <param name="project">The project.</param>
        void GetProjectMatrix(OctopusProjectViewModel project);
    }
}
