// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OctopusProjectViewModel.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   ViewModel to hold a project item for binding in the explorer view.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.ViewModels
{
    using System;

    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Model;

    using Octopus.Client.Model;

    /// <summary>
    /// ViewModel to hold a project item for binding in the explorer view.
    /// </summary>
    internal class OctopusProjectViewModel : TreeViewItemViewModel
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="OctopusProjectViewModel" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="resource">The resource.</param>
        public OctopusProjectViewModel(OctopusGroupViewModel parent, ProjectResource resource) : base(parent)
        {
            this.Resource = resource;
        }

        /// <summary>
        /// An event raised when a we require the release matrix to be loaded.
        /// </summary>
        internal event Action<OctopusProjectViewModel> OnLoadMatrix;

        /// <summary>
        /// Gets or sets the release matrix.
        /// </summary>
        /// <value>
        /// The release matrix.
        /// </value>
        public ReleaseMatrixModel ReleaseMatrix { get; set; }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        public string Image { get; set; }

        /// <summary>
        /// Gets or sets the project.
        /// </summary>
        /// <value>
        /// The project.
        /// </value>
        public ProjectResource Resource { get; set; }

        /// <summary>
        /// Invoked when the selected item need to be loaded on demand.
        /// </summary>
        protected override void LoadSelected()
        {
            if (this.OnLoadMatrix != null)
            {
                this.OnLoadMatrix.Invoke(this);
            }
        }

        /// <summary>
        /// Determines whether [is filter matched] [the filter criteria].
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>
        /// A flag indicating a match.
        /// </returns>
        protected override bool IsFilterMatched(string filter)
        {
            return string.IsNullOrEmpty(filter) || this.Resource.Name.Contains(filter);
        }
    }
}
