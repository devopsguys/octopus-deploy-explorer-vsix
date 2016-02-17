// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OctopusDeployContainerPackage.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   Abstract class to wrap a package with container functionality.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy
{
    using System.ComponentModel.Design;

    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Events;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Services;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.ViewModels;

    using Funq;

    using Microsoft.VisualStudio.Shell;

    /// <summary>
    /// Abstract class to wrap a package with container functionality.
    /// </summary>
    /// <seealso cref="Microsoft.VisualStudio.Shell.Package" />
    public abstract class OctopusDeployContainerPackage : Package
    {
        /// <summary>
        /// The container.
        /// </summary>
        private static Container container;

        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        /// <value>
        /// The container.
        /// </value>
        public static Container Container
        {
            get { return container; }
            set { container = value; }
        }

        /// <summary>
        /// Called when package is started so we can initialise.
        /// </summary>
        protected abstract void OnPackageStarted();

        /// <summary>
        /// Called when package is exited so we can clean up correctly.
        /// </summary>
        protected virtual void OnPackageExit()
        {
        }

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            this.CreateContainer();
            this.OnPackageStarted();
        }

        /// <summary>
        /// Creates the container.
        /// </summary>
        private void CreateContainer()
        {
            container = new Container();
            container.Register<ILogService>(x => new LogService());
            container.Register<IEventAggregator>(x => new EventAggregator());
            container.Register<ISettingsStoreProvider>(x => new SettingsStoreProvider(this));
            container.Register<ISettingsService>(x => new SettingsService(x.Resolve<ILogService>(), x.Resolve<ISettingsStoreProvider>()));
            container.Register<ICommandService>(x => new CommandService(this.GetService(typeof(IMenuCommandService)) as IMenuCommandService));
            container.Register<IApiClientFactory>(x => new ApiClientFactory(x.Resolve<ILogService>()));
            container.Register<IApiService>(x => new ApiService(x.Resolve<ILogService>(), x.Resolve<IEventAggregator>(), x.Resolve<IApiClientFactory>()));
            container.Register(x => new ExplorerViewModel());
            container.Register(x => new ServerSettingsViewModel(x.Resolve<ISettingsService>(), x.Resolve<ICommandService>())).ReusedWithin(ReuseScope.None);
            container.Register(x => this.FindToolWindow(typeof(OctopusDeployExplorerToolWindow), 0, true) as OctopusDeployExplorerToolWindow);
        }
    }
}
