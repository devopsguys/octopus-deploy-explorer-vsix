// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEventAggregator.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   Defines an interface to get instances of an event type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Events
{
    /// <summary>
    /// Defines an interface to get instances of an event type.
    /// </summary>
    internal interface IEventAggregator
    {
        /// <summary>
        /// Gets an instance of an event type.
        /// </summary>
        /// <typeparam name="TEventType">The type of event to get.</typeparam>
        /// <returns>An instance of an event object of type <typeparamref name="TEventType"/>.</returns>
        TEventType GetEvent<TEventType>() where TEventType : EventBase, new();
    }
}
