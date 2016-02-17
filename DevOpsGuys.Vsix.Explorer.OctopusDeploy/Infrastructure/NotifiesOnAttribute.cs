// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotifiesOnAttribute.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   This attribute is used to declare that a property should raise a notification
//   when an independent property is raising a notification.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Infrastructure
{
    using System;

    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Helpers;

    /// <summary>
    /// This attribute is used to declare that a property should raise a notification
    /// when an independent property is raising a notification.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    internal sealed class NotifiesOnAttribute : Attribute
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="NotifiesOnAttribute"/> class.
        /// </summary>
        /// <param name="name">The name of the independent property.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        public NotifiesOnAttribute(string name)
        {
            Guard.ArgumentNotNull(name, "name");
            this.Name = name;
        }

        /// <summary>
        /// Gets the name of the independent property.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// A unique identifier for this attribute.
        /// </summary>
        public override object TypeId
        {
            get
            {
                return this;
            }
        }
    }
}
