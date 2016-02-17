// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OctopusGroupViewModel.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   View model to hold a project group for binding in the explorer view.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.ViewModels
{
    using System;

    using Octopus.Client.Model;

    /// <summary>
    /// View model to hold a project group for binding in the explorer view.
    /// </summary>
    internal class OctopusGroupViewModel : TreeViewItemViewModel
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="OctopusGroupViewModel" /> class.
        /// </summary>
        /// <param name="resource">The resource.</param>
        public OctopusGroupViewModel(ProjectGroupResource resource)
            : base(null)
        {
            this.Resource = resource;
        }

        /// <summary>
        /// An event raised when a we require the children to be loaded.
        /// </summary>
        internal event Action<OctopusGroupViewModel> OnLoadChildren;

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        public string Image { get; set; }

        /// <summary>
        /// Gets or sets the group.
        /// </summary>
        /// <value>
        /// The group.
        /// </value>
        public ProjectGroupResource Resource { get; set; }

        /// <summary>
        /// Invoked when the child items need to be loaded on demand.
        /// Subclasses can override this to populate the Children collection.
        /// </summary>
        protected override void LoadChildren()
        {
            if (this.OnLoadChildren != null)
            {
                this.OnLoadChildren.Invoke(this);
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
