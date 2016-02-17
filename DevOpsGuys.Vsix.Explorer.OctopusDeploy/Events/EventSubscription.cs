﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventSubscription.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   Provides a way to retrieve a <see cref="Delegate" /> to execute an action depending
//   on the value of a second filter predicate that returns true if the action should execute.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Events
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Provides a way to retrieve a <see cref="Delegate"/> to execute an action depending
    /// on the value of a second filter predicate that returns true if the action should execute.
    /// </summary>
    /// <typeparam name="TPayload">The type to use for the generic <see cref="System.Action{TPayload}"/> and <see cref="Predicate{TPayload}"/> types.</typeparam>
    internal class EventSubscription<TPayload> : IEventSubscription
    {
        /// <summary>
        /// The action reference.
        /// </summary>
        private readonly IDelegateReference actionReference;

        /// <summary>
        /// The filter reference.
        /// </summary>
        private readonly IDelegateReference filterReference;

        /// <summary>
        /// Initialises a new instance of the <see cref="EventSubscription{TPayload}"/> class.
        /// </summary>
        /// <param name="actionReference">A reference to a delegate of type <see cref="System.Action{TPayload}" />.</param>
        /// <param name="filterReference">A reference to a delegate of type <see cref="Predicate{TPayload}" />.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="actionReference" /> or <see paramref="filterReference" /> are <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">When the target of <paramref name="actionReference" /> is not of type <see cref="System.Action{TPayload}" />,
        /// or the target of <paramref name="filterReference" /> is not of type <see cref="Predicate{TPayload}" />.</exception>
        public EventSubscription(IDelegateReference actionReference, IDelegateReference filterReference)
        {
            if (actionReference == null)
            {
                throw new ArgumentNullException("actionReference");
            }

            if (!(actionReference.Target is Action<TPayload>))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.InvalidDelegateRerefenceTypeException, typeof(Action<TPayload>).FullName), "actionReference");
            }

            if (filterReference == null)
            {
                throw new ArgumentNullException("filterReference");
            }

            if (!(filterReference.Target is Predicate<TPayload>))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.InvalidDelegateRerefenceTypeException, typeof(Predicate<TPayload>).FullName), "filterReference");
            }

            this.actionReference = actionReference;
            this.filterReference = filterReference;
        }

        /// <summary>
        /// Gets the target <see cref="System.Action{T}"/> that is referenced by the <see cref="IDelegateReference"/>.
        /// </summary>
        /// <value>An <see cref="System.Action{T}"/> or <see langword="null" /> if the referenced target is not alive.</value>
        public Action<TPayload> Action
        {
            get { return (Action<TPayload>)this.actionReference.Target; }
        }

        /// <summary>
        /// Gets the target <see cref="Predicate{T}"/> that is referenced by the <see cref="IDelegateReference"/>.
        /// </summary>
        /// <value>An <see cref="Predicate{T}"/> or <see langword="null" /> if the referenced target is not alive.</value>
        public Predicate<TPayload> Filter
        {
            get { return (Predicate<TPayload>)this.filterReference.Target; }
        }

        /// <summary>
        /// Gets or sets a <see cref="Events.SubscriptionToken"/> that identifies this <see cref="IEventSubscription"/>.
        /// </summary>
        /// <value>A token that identifies this <see cref="IEventSubscription"/>.</value>
        public SubscriptionToken SubscriptionToken { get; set; }

        /// <summary>
        /// Gets the execution strategy to publish this event.
        /// </summary>
        /// <returns>An <see cref="System.Action{T}"/> with the execution strategy, or <see langword="null" /> if the <see cref="IEventSubscription"/> is no longer valid.</returns>
        /// <remarks>
        /// If <see cref="Action"/> or <see cref="Filter"/> are no longer valid because they were
        /// garbage collected, this method will return <see langword="null" />.
        /// Otherwise it will return a delegate that evaluates the <see cref="Filter"/> and if it
        /// returns <see langword="true" /> will then call <see cref="InvokeAction"/>. The returned
        /// delegate holds hard references to the <see cref="Action"/> and <see cref="Filter"/> target
        /// <see cref="Delegate">delegates</see>. As long as the returned delegate is not garbage collected,
        /// the <see cref="Action"/> and <see cref="Filter"/> references delegates won't get collected either.
        /// </remarks>
        public virtual Action<object[]> GetExecutionStrategy()
        {
            Action<TPayload> action = this.Action;
            Predicate<TPayload> filter = this.Filter;
            if (action != null && filter != null)
            {
                return arguments =>
                {
                    TPayload argument = default(TPayload);
                    if (arguments != null && arguments.Length > 0 && arguments[0] != null)
                    {
                        argument = (TPayload)arguments[0];
                    }

                    if (filter(argument))
                    {
                        this.InvokeAction(action, argument);
                    }
                };
            }

            return null;
        }

        /// <summary>
        /// Invokes the specified <see cref="System.Action{TPayload}" /> synchronously.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="argument">The payload to pass <paramref name="action" /> while invoking it.</param>
        /// <exception cref="ArgumentNullException">An <see cref="ArgumentNullException" /> is thrown if <paramref name="action" /> is null.</exception>
        public virtual void InvokeAction(Action<TPayload> action, TPayload argument)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            action(argument);
        }
    }
}