// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReleaseMatrixEnvironment.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   A environment within the release matrix.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Model
{
    /// <summary>
    /// An environment within the release matrix.
    /// </summary>
    internal class ReleaseMatrixEnvironment
    {
        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the string format.
        /// </summary>
        /// <value>
        /// The string format.
        /// </value>
        public string StringFormat { get; set; }
    }
}
