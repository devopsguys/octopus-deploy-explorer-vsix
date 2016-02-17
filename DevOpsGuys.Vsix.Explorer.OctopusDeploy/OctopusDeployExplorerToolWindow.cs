// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OctopusDeployExplorerToolWindow.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   The explorer tool window pane.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy
{
    using System;
    using System.ComponentModel.Design;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Events;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Infrastructure.Commands;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Infrastructure.Events;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Services;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.ViewModels;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Views;

    using Microsoft.Internal.VisualStudio.PlatformUI;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.PlatformUI;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;

    /// <summary>
    /// The explorer tool window pane.
    /// </summary>
    [Guid(ExplorerToolWindowCommand.GuidString)]
    public class OctopusDeployExplorerToolWindow : ToolWindowPane, IVsWindowFrameNotify3
    {
        /// <summary>
        /// The default caption.
        /// </summary>
        private const string DefaultCaption = "Octopus Deploy Explorer";

        /// <summary>
        /// The log service.
        /// </summary>
        private readonly ILogService logService;

        /// <summary>
        /// The view model.
        /// </summary>
        private readonly ExplorerViewModel viewModel;

        /// <summary>
        /// The API service wrapper.
        /// </summary>
        private readonly IApiService apiService;

        /// <summary>
        /// The is visible.
        /// </summary>
        private bool isVisible;

        /// <summary>
        /// Initialises a new instance of the <see cref="OctopusDeployExplorerToolWindow"/> class.
        /// </summary>
        [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Microsoft.VisualStudio.Shell.ToolWindowPane.set_Caption(System.String)", Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed")]
        public OctopusDeployExplorerToolWindow() : base(null)
        {
            try
            {
                this.logService = OctopusDeployContainerPackage.Container.Resolve<ILogService>();
                this.logService.Trace("Enter");

                var eventAggregator = OctopusDeployContainerPackage.Container.Resolve<IEventAggregator>();
                eventAggregator.GetEvent<OctopusModelBuiltEvent>().Subscribe(this.OnOctopusModelBuilt);

                this.apiService = OctopusDeployContainerPackage.Container.Resolve<IApiService>();
                this.Caption = DefaultCaption;
                this.ToolBar = new CommandID(new Guid("b3afd029-1649-492a-846c-114c6761f2a9"), 0x0101);
                this.ToolBarLocation = (int)VSTWT_LOCATION.VSTWT_TOP;
                this.viewModel = ViewModelLocatorService.ExplorerViewModel;

                this.Content = new ExplorerView { DataContext = this.viewModel };
            }
            catch (Exception ex)
            {
                this.logService.Error("Exception occured in OctopusDeployExplorerToolWindow.Constructor", ex);
            }

            this.logService.Trace("Exit");
        }

        /// <summary>
        /// Override this if you want to support search in your window. You must also override other functions from the <see cref="T:Microsoft.VisualStudio.Shell.Interop.IVsWindowSearch" /> interface, like CreateSearch, etc.
        /// </summary>
        public override bool SearchEnabled
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is visible.
        /// </summary>
        /// <value>
        /// <c>True</c> if this instance is visible; otherwise, <c>false</c>.
        /// </value>
        private bool IsVisible
        {
            get
            {
                return this.isVisible;
            }

            set
            {
                if (this.isVisible != value)
                {
                    this.isVisible = value;
                    if (this.isVisible)
                    {
                        this.Refresh();
                    }
                }
            }
        }
        
        /// <summary>
        /// Refresh the Explorer tool window.
        /// </summary>
        public void Refresh()
        {
            this.logService.Trace("Enter");
            this.viewModel.IsRefreshing = true;
            System.Threading.Tasks.Task.Run(() => this.apiService.GetGroups());
            this.logService.Trace("Exit");
        }

        /// <summary>
        /// Creates the search.
        /// </summary>
        /// <param name="cookie">The cookie.</param>
        /// <param name="searchQuery">The search query.</param>
        /// <param name="searchCallback">The search call back.</param>
        /// <returns>The <see cref="IVsSearchTask"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1061:DoNotHideBaseClassMethods", Justification = "Reviewed. Suppression is OK here.")]
        public override IVsSearchTask CreateSearch(uint cookie, IVsSearchQuery searchQuery, IVsSearchCallback searchCallback)
        {
            return new OctopusDeployProjectSearchTask(cookie, searchQuery, searchCallback, this.ApplyFilter);
        }

        /// <summary>
        /// Clears the pane of the results from a previously completed or partial search.
        /// </summary>
        public override void ClearSearch()
        {
            this.logService.Trace("Enter");
            this.RemoveFilter();
            this.logService.Trace("Exit");
        }

        /// <summary>
        /// Provides the search settings.
        /// </summary>
        /// <param name="searchSettings">The search settings.</param>
        [SuppressMessage("Microsoft.Design", "CA1061:DoNotHideBaseClassMethods", Justification = "Reviewed. Suppression is OK here.")]
        public override void ProvideSearchSettings(IVsUIDataSource searchSettings)
        {
            base.ProvideSearchSettings(searchSettings);

            Utilities.SetValue(searchSettings, SearchSettingsDataSource.PropertyNames.ControlMinWidth, 200U);
            Utilities.SetValue(searchSettings, SearchSettingsDataSource.PropertyNames.ControlMaxWidth, uint.MaxValue);
            Utilities.SetValue(searchSettings, SearchSettingsDataSource.PropertyNames.SearchWatermark, "Filter projects");
        }

        /// <summary>
        /// Raises the Close event.
        /// </summary>
        /// <param name="pgrfSaveOptions">Specifies options for saving window content. Values are taken from the <see cref="T:Microsoft.VisualStudio.Shell.Interop.__FRAMECLOSE" /> enumeration.</param>
        /// <returns>
        /// If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it returns an error code.
        /// </returns>
        public int OnClose(ref uint pgrfSaveOptions)
        {
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Notifies the VSPackage that a window's docked state is being altered.
        /// </summary>
        /// <param name="fDockable">True if the window frame is being docked.</param>
        /// <param name="x">Horizontal position of undocked window.</param>
        /// <param name="y">Vertical position of undocked window.</param>
        /// <param name="w">Width of undocked window.</param>
        /// <param name="h">Height of undocked window.</param>
        /// <returns>
        /// If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it returns an error code.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        public int OnDockableChange(int fDockable, int x, int y, int w, int h)
        {
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Notifies the VSPackage that a window is being moved.
        /// </summary>
        /// <param name="x">New horizontal position.</param>
        /// <param name="y">New vertical position.</param>
        /// <param name="w">New window width.</param>
        /// <param name="h">New window height.</param>
        /// <returns>
        /// If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it returns an error code.
        /// </returns>
        public int OnMove(int x, int y, int w, int h)
        {
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Notifies the VSPackage of a change in the window's display state.
        /// </summary>
        /// <param name="fShow">Specifies the reason for the display state change. Value taken from the <see cref="T:Microsoft.VisualStudio.Shell.Interop.__FRAMESHOW" /> enumeration.</param>
        /// <returns>
        /// If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it returns an error code.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        public int OnShow(int fShow)
        {
            // Track the visibility of this tool window.
            switch ((__FRAMESHOW)fShow)
            {
                case __FRAMESHOW.FRAMESHOW_WinShown:
                    this.IsVisible = true;
                    break;

                case __FRAMESHOW.FRAMESHOW_WinHidden:
                    this.IsVisible = false;
                    break;
            }

            return VSConstants.S_OK;
        }

        /// <summary>
        /// Notifies the VSPackage that a window is being resized.
        /// </summary>
        /// <param name="x">New horizontal position.</param>
        /// <param name="y">New vertical position.</param>
        /// <param name="w">New window width.</param>
        /// <param name="h">New window height.</param>
        /// <returns>
        /// If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it returns an error code.
        /// </returns>
        public int OnSize(int x, int y, int w, int h)
        {
            return VSConstants.S_OK;
        }

        /// <summary>
        /// This is called after our control has been created and sited.
        /// This is a good place to initialize the control with data gathered
        /// from Visual Studio services.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Microsoft.VisualStudio.Shell.Interop.IVsWindowFrame.SetProperty(System.Int32,System.Object)", Justification = "Reviewed. Suppression is OK here.")]
        public override void OnToolWindowCreated()
        {
            this.logService.Trace("Enter");
            base.OnToolWindowCreated();

            // Register for events to this window.
            var result = ((IVsWindowFrame)this.Frame).SetProperty((int)__VSFPROPID.VSFPROPID_ViewHelper, this);
            this.logService.Trace("Exit");
        }

        /// <summary>
        /// An event handler called when the <see cref="IEventAggregator" /> catches 
        /// an <see cref="OctopusModelBuiltEvent" /> event.
        /// </summary>
        /// <param name="octopusDeployModel">The octopus deploy model.</param>
        private void OnOctopusModelBuilt(OctopusViewModel octopusDeployModel)
        {
            this.viewModel.OctopusViewModel = octopusDeployModel;
            this.viewModel.IsRefreshing = false;
        }

        /// <summary>
        /// Applies the search filter to the tree nodes.
        /// </summary>
        /// <param name="filter">The filter.</param>
        private void ApplyFilter(string filter)
        {
            this.logService.Trace("Enter");
            foreach (var node in this.viewModel.OctopusViewModel.Groups)
            {
                node.ApplyFilter(filter);
            }

            this.logService.Trace("Exit");
        }

        /// <summary>
        /// Removes the filter.
        /// </summary>
        private void RemoveFilter()
        {
            this.logService.Trace("Enter");
            foreach (var node in this.viewModel.OctopusViewModel.Groups)
            {
                node.ApplyFilter(string.Empty);
            }

            this.logService.Trace("Exit");
        }
    }
}
