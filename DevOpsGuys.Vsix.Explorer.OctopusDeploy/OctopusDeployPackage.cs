// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OctopusDeployPackage.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   This is the class that implements the package exposed by this assembly.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy
{
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Infrastructure.Commands;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Services;

    using EnvDTE;

    using Microsoft.VisualStudio.Shell;

    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(OctopusDeployExplorerToolWindow), Style = VsDockStyle.Tabbed, Window = Constants.vsWindowKindOutput)]
    [Guid("94f16d76-eaa5-4118-99a0-421f83c78110")]
    [ProvideBindingPath]
    public sealed class OctopusDeployPackage : OctopusDeployContainerPackage
    {
        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed")]
        protected override void OnPackageStarted()
        {
            RegisterCommands();
        }
        
        /// <summary>
        /// Register the package commands (which must exist in the .VSCT file).
        /// </summary>
        private static void RegisterCommands()
        {
            var commandService = Container.Resolve<ICommandService>();
            if (commandService == null)
            {
                return;
            }

            // Create the individual commands, which internally register for command events.
            commandService.Add(new ExplorerToolWindowCommand());
            commandService.Add(new ExplorerRefreshButtonCommand());
            commandService.Add(new ExplorerSettingsButtonCommand());
        }
    }
}
