// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiService.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   Implementation of the Api service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Services
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Events;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Helpers;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Infrastructure.Events;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Model;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.ViewModels;

    using Octopus.Client.Exceptions;
    using Octopus.Client.Model;

    /// <summary>
    /// Implementation of the API service.
    /// </summary>
    /// <seealso cref="DevOpsGuys.Vsix.Explorer.OctopusDeploy.Services.IApiService" />
    internal class ApiService : IApiService
    {
        /// <summary>
        /// The log service.
        /// </summary>
        private readonly ILogService logService;

        /// <summary>
        /// The event aggregator.
        /// </summary>
        private readonly IEventAggregator eventAggregator;

        /// <summary>
        /// The API client factory.
        /// </summary>
        private readonly IApiClientFactory apiClientFactory;

        /// <summary>
        /// Initialises a new instance of the <see cref="ApiService" /> class.
        /// </summary>
        /// <param name="logService">The log service.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="apiClientFactory">The API client factory.</param>
        public ApiService(ILogService logService, IEventAggregator eventAggregator, IApiClientFactory apiClientFactory)
        {
            Guard.ArgumentNotNull(logService, "logService");
            Guard.ArgumentNotNull(eventAggregator, "eventAggregator");
            Guard.ArgumentNotNull(apiClientFactory, "apiClientFactory");
            this.logService = logService;
            this.eventAggregator = eventAggregator;
            this.apiClientFactory = apiClientFactory;

            this.logService.Trace("Exit");
        }

        /// <summary>
        /// Gets the initial groups defined in the Octopus Deploy server.
        /// </summary>
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "To Review")]
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "To Review")]
        public void GetGroups()
        {
            this.logService.Trace("Enter");
            var client = this.apiClientFactory.GetClient();
            if (client == null)
            {
                return;
            }

            var model = new OctopusViewModel();

            try
            {
                var rootDocument = client.RootDocument;
                var versionNumber = Common.GetVersionNumber(rootDocument.Version);
                model.ServerVersion = "Octopus Deploy: " + rootDocument.Version;
                this.logService.Info(model.ServerVersion);

                if (versionNumber.Major < 2 || (versionNumber.Major == 2 && versionNumber.Minor < 6))
                {
                    model.ExceptionMessage = OctopusDeploy.Resources.ODE1003;
                }
                else
                {
                    var groups = client.List<ProjectGroupResource>(rootDocument.Links["ProjectGroups"]);

                    foreach (var group in groups.Items.Select(resource => new OctopusGroupViewModel(resource)))
                    {
                        group.OnLoadChildren += this.GetProjectsIntoGroup;
                        model.Groups.Add(@group);
                    }

                    this.logService.Info("Groups Loaded");
                }
            }
            catch (OctopusSecurityException ex)
            {
                model.ExceptionMessage = OctopusDeploy.Resources.ODE1004;
                model.ExceptionMessageButtonText = OctopusDeploy.Resources.EditSettingsButtonText;
                this.logService.Error(ex);
            }
            catch (OctopusDeserializationException ex)
            {
                model.ExceptionMessage = OctopusDeploy.Resources.ODE1005;
                model.ExceptionMessageButtonText = OctopusDeploy.Resources.EditSettingsButtonText;
                this.logService.Error(ex);
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("Unable to connect to the Octopus Deploy server.", StringComparison.OrdinalIgnoreCase))
                {
                    model.ExceptionMessage = OctopusDeploy.Resources.ODE1002 + ex.InnerException.Message;
                    this.logService.Error(ex);
                }
                else
                {
                    model.ExceptionMessage = OctopusDeploy.Resources.ODE1006;
                    this.logService.Error(ex);
                }
            }

            if (!string.IsNullOrWhiteSpace(model.ExceptionMessage))
            {
                this.logService.Warn(model.ExceptionMessage);
                model.ExceptionMessage = OctopusDeploy.Resources.ExceptionStartMessage + Environment.NewLine + Environment.NewLine + model.ExceptionMessage;
            }

            this.eventAggregator.GetEvent<OctopusModelBuiltEvent>().Publish(model);
            this.logService.Trace("Exit");
        }

        /// <summary>
        /// Gets the projects defined in the group and populates the view model.
        /// </summary>
        /// <param name="group">The group.</param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "To Review")]
        public void GetProjectsIntoGroup(OctopusGroupViewModel group)
        {
            this.logService.Trace("Enter");

            Guard.ArgumentNotNull(group, "group");

            var client = this.apiClientFactory.GetClient();
            if (client == null)
            {
                return;
            }

            try
            {
                group.Children.Clear();
                var projects = client.List<ProjectResource>(group.Resource.Links["Projects"]).Items;

                // Project Details
                foreach (var project in projects.Select(item => new OctopusProjectViewModel(group, item)))
                {
                    project.OnLoadMatrix += this.GetProjectMatrix;
                    group.Children.Add(project);
                }

                this.logService.Info("Projects Loaded");
            }
            catch (Exception ex)
            {
                this.logService.Error(ex);
            }

            this.logService.Trace("Exit");
        }

        /// <summary>
        /// Gets the project matrix and populates the view model.
        /// </summary>
        /// <param name="project">The project.</param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "To Review")]
        public void GetProjectMatrix(OctopusProjectViewModel project)
        {
            this.logService.Trace("Enter");

            Guard.ArgumentNotNull(project, "project");

            var client = this.apiClientFactory.GetClient();
            if (client == null)
            {
                return;
            }

            try
            {
                var rootDocument = client.RootDocument;
                var progression = client.Get<ProgressionResource>(project.Resource.Links["Progression"]);

                if (rootDocument.Version.StartsWith("2.", StringComparison.OrdinalIgnoreCase))
                {
                    project.ReleaseMatrix = this.CreateV2ReleaseMatrix(progression);
                }
                else
                {
                    project.ReleaseMatrix = this.CreateV3ReleaseMatrix(progression);
                }

                this.logService.Info("Matrix Loaded");
            }
            catch (Exception ex)
            {
                this.logService.Error(ex);
            }

            this.logService.Trace("Exit");
        }

        /// <summary>
        /// Creates the release matrix for a V2 server.
        /// </summary>
        /// <param name="progression">The progression.</param>
        /// <returns>
        /// The <see cref="ReleaseMatrixModel" />.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "To Review")]
        private ReleaseMatrixModel CreateV2ReleaseMatrix(ProgressionResource progression)
        {
            this.logService.Trace("Enter");

            var result = new ReleaseMatrixModel { Environments = new Collection<ReleaseMatrixEnvironment>(), Releases = new Collection<object[]>() };

            try
            {
                var index = 0;
                var distinctEnvironmentsFromReleases = progression.Releases.SelectMany(x => x.Deployments).Select(x => x.Key).Distinct();
                progression.Environments.ForEach(
                    x =>
                    {
                        if (distinctEnvironmentsFromReleases.Contains(x.Id))
                        {
                            result.Environments.Add(new ReleaseMatrixEnvironment { Index = index, Id = x.Id, Name = x.Name });
                            index++;
                        }
                    });

                this.logService.Info("Environments Loaded");

                foreach (var release in progression.Releases)
                {
                    var row = new object[progression.Environments.Count];

                    foreach (var deployment in release.Deployments)
                    {
                        var environmentColumnIndex = result.Environments.First(x => x.Id == deployment.Key).Index;
                        row[environmentColumnIndex] = deployment.Value;
                    }

                    result.Releases.Add(row);
                }

                this.logService.Info("Releases Loaded");
            }
            catch (Exception ex)
            {
                this.logService.Error(ex);
            }

            this.logService.Trace("Exit");

            return result;
        }

        /// <summary>
        /// Creates the release matrix for a V3 server.
        /// </summary>
        /// <param name="progression">The progression.</param>
        /// <returns>
        /// The <see cref="ReleaseMatrixModel" />.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "To Review")]
        private ReleaseMatrixModel CreateV3ReleaseMatrix(ProgressionResource progression)
        {
            this.logService.Trace("Enter");

            var result = new ReleaseMatrixModel { Environments = new Collection<ReleaseMatrixEnvironment>(), Releases = new Collection<object[]>() };

            try
            {
                var index = 0;
                progression.Environments.ForEach(
                    x =>
                    {
                        result.Environments.Add(new ReleaseMatrixEnvironment { Index = index, Id = x.Id, Name = x.Name });
                        index++;
                    });

                this.logService.Info("Environments Loaded");

                foreach (var release in progression.Releases)
                {
                    var row = new object[progression.Environments.Count];

                    foreach (var deployment in release.Deployments)
                    {
                        var environmentColumnIndex = result.Environments.First(x => x.Id == deployment.Key).Index;
                        row[environmentColumnIndex] = deployment.Value;
                    }

                    result.Releases.Add(row);
                }

                this.logService.Info("Releases Loaded");
            }
            catch (Exception ex)
            {
                this.logService.Error(ex);
            }

            this.logService.Trace("Exit");

            return result;
        }
    }
}
