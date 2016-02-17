// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DynamicRecordProperty.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   Model for properties in a dynamic record.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Model
{
    /// <summary>
    /// Model for properties in a dynamic record.
    /// </summary>
    internal class DynamicRecordProperty
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="DynamicRecordProperty"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public DynamicRecordProperty(string name, object value)
        {
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public object Value { get; set; }
    }
}
