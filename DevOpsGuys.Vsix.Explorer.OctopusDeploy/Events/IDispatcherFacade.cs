// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDispatcherFacade.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   Defines the interface for invoking methods through a Dispatcher Facade.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Events
{
    using System;

    /// <summary>
    /// Defines the interface for invoking methods through a Dispatcher Facade.
    /// </summary>
    internal interface IDispatcherFacade
    {
        /// <summary>
        /// Dispatches an invocation to the method received as parameter.
        /// </summary>
        /// <param name="method">Method to be invoked.</param>
        /// <param name="arg">Arguments to pass to the invoked method.</param>
        void BeginInvoke(Delegate method, object arg);
    }
}