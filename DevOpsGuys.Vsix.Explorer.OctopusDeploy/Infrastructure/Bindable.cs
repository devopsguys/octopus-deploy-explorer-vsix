// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Bindable.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   The base class for binding objects.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;

    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Helpers;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.UI;

    /// <summary>
    /// The base class for binding objects.
    /// </summary>
    internal abstract class Bindable : INotifyPropertyChanged
    {
        /// <summary>
        /// A dictionary holding a set of property/value pairs.
        /// </summary>
        private readonly Dictionary<string, object> propertyBackingDictionary = new Dictionary<string, object>();

        /// <summary>
        /// The dependent lookup.
        /// </summary>
        private ILookup<string, string> dependentLookup;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets a lookup structure containing all independent/dependent property pairs based on <see cref="NotifiesOnAttribute" /> definitions.
        /// </summary>
        /// <value>
        /// The dependent lookup.
        /// </value>
        private ILookup<string, string> DependentLookup
        {
            get
            {
                return this.dependentLookup ?? (this.dependentLookup = (from p in this.GetType().GetProperties()
                                                                        let attrs = p.GetCustomAttributes(typeof(NotifiesOnAttribute), false)
                                                                        from NotifiesOnAttribute a in attrs
                                                                        select new { Independent = a.Name, Dependent = p.Name }).ToLookup(i => i.Independent, d => d.Dependent));
            }
        }

        /// <summary>
        /// Gets the property value for the specified property name.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>
        /// The property value if set, otherwise the default for its type.
        /// </returns>
        /// <exception cref="ArgumentNullException">If the propertyName is null.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Todo")]
        protected T GetPropertyValue<T>([CallerMemberName] string propertyName = null)
        {
            Guard.ArgumentNotNull(propertyName, "propertyName");
            object value;
            if (this.propertyBackingDictionary.TryGetValue(propertyName, out value))
            {
                return (T)value;
            }

            return default(T);
        }

        /// <summary>
        /// Sets the property value for the specified property name if the value has changed. On
        /// change a <see cref="INotifyPropertyChanged.PropertyChanged" /> event will be raised.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="newValue">The new property value.</param>
        /// <param name="propertyName">The property value.</param>
        /// <returns>
        /// True if the value was changed, otherwise false.
        /// </returns>
        /// <exception cref="ArgumentNullException">If the propertyName is null.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Todo")]
        protected bool SetPropertyValue<T>(T newValue, [CallerMemberName] string propertyName = null)
        {
            Guard.ArgumentNotNull(propertyName, "propertyName");
            if (EqualityComparer<T>.Default.Equals(newValue, this.GetPropertyValue<T>(propertyName)))
            {
                return false;
            }

            this.propertyBackingDictionary[propertyName] = newValue;
            this.RaisePropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged" /> event.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate", Justification = "Todo")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Todo")]
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            Guard.ArgumentNotNull(propertyName, "propertyName");
            var propertyChanged = this.PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));

                foreach (var dependentPropertyName in this.DependentLookup[propertyName])
                {
                    if (dependentPropertyName != null)
                    {
                        this.RaisePropertyChanged(dependentPropertyName);
                    }
                }
            }
        }
    }
}
