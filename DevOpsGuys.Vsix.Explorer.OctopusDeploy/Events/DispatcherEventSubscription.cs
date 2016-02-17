// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DispatcherEventSubscription.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   The dispatcher event subscription.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Events
{
    using System;
    using System.Windows.Threading;

    /// <summary>
    /// The dispatcher event subscription.
    /// </summary>
    /// <typeparam name="TPayload">The type of the payload.</typeparam>
    /// <seealso cref="DevOpsGuys.Vsix.Explorer.OctopusDeploy.Events.EventSubscription{TPayload}" />
    internal class DispatcherEventSubscription<TPayload> : EventSubscription<TPayload>
    {
        /// <summary>
        /// The dispatcher.
        /// </summary>
        private readonly IDispatcherFacade dispatcher;

        /// <summary>
        /// Initialises a new instance of the <see cref="DispatcherEventSubscription{TPayload}"/> class.
        /// </summary>
        /// <param name="actionReference">A reference to a delegate of type <see cref="System.Action{TPayload}" />.</param>
        /// <param name="filterReference">A reference to a delegate of type <see cref="Predicate{TPayload}" />.</param>
        /// <param name="dispatcher">The dispatcher to use when executing the <paramref name="actionReference" /> delegate.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="actionReference" /> or <see paramref="filterReference" /> are <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">When the target of <paramref name="actionReference" /> is not of type <see cref="System.Action{TPayload}" />,
        /// or the target of <paramref name="filterReference" /> is not of type <see cref="Predicate{TPayload}" />.</exception>
        public DispatcherEventSubscription(IDelegateReference actionReference, IDelegateReference filterReference, IDispatcherFacade dispatcher)
            : base(actionReference, filterReference)
        {
            this.dispatcher = dispatcher;
        }

        /// <summary>
        /// Invokes the specified <see cref="System.Action{TPayload}"/> asynchronously in the specified <see cref="Dispatcher"/>.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="argument">The payload to pass <paramref name="action"/> while invoking it.</param>
        public override void InvokeAction(Action<TPayload> action, TPayload argument)
        {
            this.dispatcher.BeginInvoke(action, argument);
        }
    }
}