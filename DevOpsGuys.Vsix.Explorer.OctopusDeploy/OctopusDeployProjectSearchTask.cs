// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OctopusDeployProjectSearchTask.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   A class implementing <see cref="VsSearchTask" /> in order to search projects.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy
{
    using System;

    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;

    /// <summary>
    /// A class implementing <see cref="VsSearchTask"/> in order to search projects.
    /// </summary>
    internal class OctopusDeployProjectSearchTask : VsSearchTask
    {
        /// <summary>
        /// The call back.
        /// </summary>
        private readonly Action<string> callback;

        /// <summary>
        /// Initialises a new instance of the <see cref="OctopusDeployProjectSearchTask" /> class.
        /// </summary>
        /// <param name="cookie">The cookie.</param>
        /// <param name="searchQuery">The search query.</param>
        /// <param name="searchCallback">The search call back.</param>
        /// <param name="callback">The call back.</param>
        internal OctopusDeployProjectSearchTask(uint cookie, IVsSearchQuery searchQuery, IVsSearchCallback searchCallback, Action<string> callback) : base(cookie, searchQuery, searchCallback)
        {
            this.callback = callback;
        }

        /// <summary>
        /// Performs the search task.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed")]
        protected override void OnStartSearch()
        {
            this.ErrorCode = VSConstants.S_OK;

            try
            {
                this.callback(this.SearchQuery.SearchString);
            }
            catch (Exception)
            {
                this.ErrorCode = VSConstants.E_FAIL;
            }

            base.OnStartSearch();
        }

        /// <summary>
        /// Called on the UI thread when the search is stopped. Override to do task-specific stop actions.
        /// </summary>
        protected override void OnStopSearch()
        {
            this.SearchResults = 0;
        }
    }
}
