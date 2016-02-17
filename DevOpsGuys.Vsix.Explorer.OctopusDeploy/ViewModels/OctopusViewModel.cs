// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OctopusViewModel.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   Model to hold everything for binding in the explorer view.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Infrastructure;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.UI;

    /// <summary>
    /// Model to hold everything for binding in the explorer view.
    /// </summary>
    internal class OctopusViewModel : Bindable
    {
        /// <summary>
        /// The groups.
        /// </summary>
        private readonly ObservableCollection<OctopusGroupViewModel> groups;

        /// <summary>
        /// Initialises a new instance of the <see cref="OctopusViewModel"/> class.
        /// </summary>
        public OctopusViewModel()
        {
            this.groups = new ObservableCollection<OctopusGroupViewModel>();
        }

        /// <summary>
        /// Gets or sets the exception message.
        /// </summary>
        /// <value>
        /// The exception message.
        /// </value>
        public string ExceptionMessage { get; set; }

        /// <summary>
        /// Gets or sets the exception message button text.
        /// </summary>
        /// <value>
        /// The exception message button text.
        /// </value>
        public string ExceptionMessageButtonText { get; set; }

        /// <summary>
        /// Gets or sets the server.
        /// </summary>
        /// <value>
        /// The server.
        /// </value>
        public string ServerVersion { get; set; }

        /// <summary>
        /// Gets or sets the exception resolution command.
        /// </summary>
        /// <value>
        /// The exception resolution command.
        /// </value>
        public ICommand ExceptionResolutionCommand { get; set; }

        /// <summary>
        /// Gets the groups.
        /// </summary>
        /// <value>
        /// The groups.
        /// </value>
        public ObservableCollection<OctopusGroupViewModel> Groups
        {
            get
            {
                return this.groups;
            }
        }
    }
}
