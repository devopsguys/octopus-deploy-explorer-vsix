// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DynamicRecord.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   Model for dynamic records to bind to datagrid.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Model
{
    using System.Collections.ObjectModel;

    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Helpers;

    /// <summary>
    /// Model for dynamic records to bind to data grid.
    /// </summary>
    internal class DynamicRecord
    {
        /// <summary>
        /// The properties.
        /// </summary>
        private readonly ObservableCollection<DynamicRecordProperty> properties = new ObservableCollection<DynamicRecordProperty>();

        /// <summary>
        /// Initialises a new instance of the <see cref="DynamicRecord"/> class.
        /// </summary>
        /// <param name="properties">The properties.</param>
        public DynamicRecord(params DynamicRecordProperty[] properties)
        {
            Guard.ArgumentNotNull(properties, "properties");
            foreach (var property in properties)
            {
                this.Properties.Add(property);
            }
        }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        public ObservableCollection<DynamicRecordProperty> Properties
        {
            get
            {
                return this.properties;
            }
        }
    }
}
