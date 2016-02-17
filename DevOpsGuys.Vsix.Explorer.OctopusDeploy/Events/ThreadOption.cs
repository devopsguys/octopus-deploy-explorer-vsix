// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThreadOption.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   Specifies on which thread a <see cref="CompositePresentationEvent{TPayload}" /> subscriber will be called.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Events
{
    /// <summary>
    /// Specifies on which thread a <see cref="CompositePresentationEvent{TPayload}"/> subscriber will be called.
    /// </summary>
    internal enum ThreadOption
    {
        /// <summary>
        /// The call is done on the same thread on which the <see cref="CompositePresentationEvent{TPayload}"/> was published.
        /// </summary>
        PublisherThread,

        /// <summary>
        /// The call is done on the UI thread.
        /// </summary>
        UIThread,

        /// <summary>
        /// The call is done asynchronously on a background thread.
        /// </summary>
        BackgroundThread
    }
}