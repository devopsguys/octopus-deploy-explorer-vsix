// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultDispatcher.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   Wraps the Deployment Dispatcher.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Events
{
    using System;
    using System.Windows.Threading;

    /// <summary>
    /// Wraps the Deployment Dispatcher.
    /// </summary>
    internal class DefaultDispatcher : IDispatcherFacade
    {
        /// <summary>
        /// Forwards the BeginInvoke to the current deployment's <see cref="System.Windows.Threading.Dispatcher"/>.
        /// </summary>
        /// <param name="method">Method to be invoked.</param>
        /// <param name="arg">Arguments to pass to the invoked method.</param>
        public void BeginInvoke(Delegate method, object arg)
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(method, arg);
        }
    }
}