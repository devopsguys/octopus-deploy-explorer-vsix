// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExplorerToolWindowCommand.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   Command handler for the tool window.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Infrastructure.Commands
{
    using System;
    using System.ComponentModel.Design;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.VisualStudio.Shell.Interop;

    /// <summary>
    /// Command handler for the tool window.
    /// </summary>
    internal sealed class ExplorerToolWindowCommand : BaseCommand
    {
        /// <summary>
        /// The explorer tool window.
        /// </summary>
        public const int Id = 0x2001;

        /// <summary>
        /// The explorer tool window.
        /// </summary>
        public const string GuidString = "f6d1ca67-427c-41c4-b424-c5ce7ccd32b9";
        
        /// <summary>
        /// The explorer tool window unique identifier.
        /// </summary>
        public static readonly Guid Guid = new Guid(GuidString);

        /// <summary>
        /// Initialises a new instance of the <see cref="ExplorerToolWindowCommand" /> class.
        /// </summary>
        internal ExplorerToolWindowCommand() : base(new CommandID(Guid, Id))
        {
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Microsoft.VisualStudio.Shell.Interop.IVsWindowFrame.Show", Justification = "Reviewed")]
        protected override void OnExecute()
        {
            base.OnExecute();
            var frame = OctopusDeployContainerPackage.Container.Resolve<OctopusDeployExplorerToolWindow>().Frame as IVsWindowFrame;
            if (frame != null)
            {
                frame.Show();
            } 
        }
    }
}
