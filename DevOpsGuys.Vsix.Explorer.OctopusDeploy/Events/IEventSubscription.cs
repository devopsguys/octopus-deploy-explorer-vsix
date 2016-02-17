// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEventSubscription.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   Defines a contract for an event subscription to be used by <see cref="EventBase" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Events
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Defines a contract for an event subscription to be used by <see cref="EventBase" />.
    /// </summary>
    internal interface IEventSubscription
    {
        /// <summary>
        /// Gets or sets a <see cref="Events.SubscriptionToken"/> that identifies this <see cref="IEventSubscription"/>.
        /// </summary>
        /// <value>A token that identifies this <see cref="IEventSubscription"/>.</value>
        SubscriptionToken SubscriptionToken { get; set; }

        /// <summary>
        /// Gets the execution strategy to publish this event.
        /// </summary>
        /// <returns>An <see cref="Action{T}"/> with the execution strategy, or <see langword="null" /> if the <see cref="IEventSubscription"/> is no longer valid.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Preferred")]
        Action<object[]> GetExecutionStrategy();
    }
}