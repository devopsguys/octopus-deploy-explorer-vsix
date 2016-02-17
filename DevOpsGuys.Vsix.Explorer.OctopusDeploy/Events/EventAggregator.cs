// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventAggregator.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   Implements <see cref="IEventAggregator" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Events
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Implements <see cref="IEventAggregator"/>.
    /// </summary>
    internal class EventAggregator : IEventAggregator
    {
        /// <summary>
        /// The events.
        /// </summary>
        private readonly Dictionary<Type, EventBase> events = new Dictionary<Type, EventBase>();

        /// <summary>
        /// Gets the single instance of the event managed by this EventAggregator. Multiple calls to this method with the same <typeparamref name="TEventType"/> returns the same event instance.
        /// </summary>
        /// <typeparam name="TEventType">The type of event to get. This must inherit from <see cref="EventBase"/>.</typeparam>
        /// <returns>A singleton instance of an event object of type <typeparamref name="TEventType"/>.</returns>
        public TEventType GetEvent<TEventType>() where TEventType : EventBase, new()
        {
            EventBase existingEvent;

            if (this.events.TryGetValue(typeof(TEventType), out existingEvent))
            {
                return (TEventType)existingEvent;
            }

            var newEvent = new TEventType();
            this.events[typeof(TEventType)] = newEvent;

            return newEvent;
        }
    }
}
