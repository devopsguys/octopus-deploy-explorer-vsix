// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubscriptionToken.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   Subscription token returned from <see cref="EventBase" /> on subscribe.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Events
{
    using System;

    /// <summary>
    /// Subscription token returned from <see cref="EventBase"/> on subscribe.
    /// </summary>
    internal class SubscriptionToken : IEquatable<SubscriptionToken>
    {
        /// <summary>
        /// The token.
        /// </summary>
        private readonly Guid token;

        /// <summary>
        /// Initialises a new instance of the <see cref="SubscriptionToken"/> class.
        /// </summary>
        public SubscriptionToken()
        {
            this.token = Guid.NewGuid();
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        ///   <see langword="true" /> If the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.
        /// </returns>
        public bool Equals(SubscriptionToken other)
        {
            if (other == null)
            {
                return false;
            }

            return SubscriptionToken.Equals(this.token, other.token);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />.</param>
        /// <returns>
        /// True if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">The <paramref name="obj" /> parameter is null.</exception>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return this.Equals(obj as SubscriptionToken);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object" />.
        /// </returns>
        public override int GetHashCode()
        {
            return this.token.GetHashCode();
        }
    }
}