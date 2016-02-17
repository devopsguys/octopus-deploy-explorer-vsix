// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDelegateReference.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   Represents a reference to a <see cref="Delegate" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Events
{
    using System;

    /// <summary>
    /// Represents a reference to a <see cref="Delegate"/>.
    /// </summary>
    internal interface IDelegateReference
    {
        /// <summary>
        /// Gets the referenced <see cref="Delegate" /> object.
        /// </summary>
        /// <value>A <see cref="Delegate"/> instance if the target is valid; otherwise <see langword="null"/>.</value>
        Delegate Target { get; }
    }
}