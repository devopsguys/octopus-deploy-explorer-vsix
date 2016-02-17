// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReleaseMatrixModel.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   The release matrix model that is bound to the data grid.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Model
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <summary>
    /// The release matrix model that is bound to the data grid.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "Preferred name")]
    internal sealed class ReleaseMatrixModel : IEnumerable<object[]>
    {
        /// <summary>
        /// Gets or sets the environments.
        /// </summary>
        /// <value>
        /// The columns.
        /// </value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Todo")]
        public Collection<ReleaseMatrixEnvironment> Environments { get; set; }

        /// <summary>
        /// Gets or sets the releases.
        /// </summary>
        /// <value>
        /// The rows.
        /// </value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Todo")]
        public Collection<object[]> Releases { get; set; }

        /// <summary>
        /// Returns an enumerator that iterates through the releases.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the releases.
        /// </returns>
        public IEnumerator<object[]> GetEnumerator()
        {
            return this.Releases.AsEnumerable().GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the releases.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the releases.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Releases.AsEnumerable().GetEnumerator();
        }
    }
}
