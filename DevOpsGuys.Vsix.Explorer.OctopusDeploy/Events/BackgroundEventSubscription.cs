// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BackgroundEventSubscription.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   Extends <see cref="EventSubscription{TPayload}.Action" /> to invoke the <see cref="EventSubscription{TPayload}" /> delegate in a background thread.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Events
{
    using System;
    using System.Threading;

    /// <summary>
    /// Extends <see cref="EventSubscription{TPayload}.Action"/> to invoke the <see cref="EventSubscription{TPayload}"/> delegate in a background thread.
    /// </summary>
    /// <typeparam name="TPayload">The type to use for the generic <see cref="EventSubscription{TPayload}"/> types.</typeparam>
    internal class BackgroundEventSubscription<TPayload> : EventSubscription<TPayload>
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="BackgroundEventSubscription{TPayload}"/> class.
        /// </summary>
        /// <param name="actionReference">A reference to a delegate of type <see cref="System.Action{TPayload}" />.</param>
        /// <param name="filterReference">A reference to a delegate of type <see cref="Predicate{TPayload}" />.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="actionReference" /> or <see paramref="filterReference" /> are <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">When the target of <paramref name="actionReference" /> is not of type <see cref="System.Action{TPayload}" />,
        /// or the target of <paramref name="filterReference" /> is not of type <see cref="Predicate{TPayload}" />.</exception>
        public BackgroundEventSubscription(IDelegateReference actionReference, IDelegateReference filterReference)
            : base(actionReference, filterReference)
        {
        }

        /// <summary>
        /// Invokes the specified <see cref="System.Action{TPayload}"/> in an asynchronous thread by using a <see cref="ThreadPool"/>.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="argument">The payload to pass <paramref name="action"/> while invoking it.</param>
        public override void InvokeAction(Action<TPayload> action, TPayload argument)
        {
            ThreadPool.QueueUserWorkItem((o) => action(argument));
        }
    }
}