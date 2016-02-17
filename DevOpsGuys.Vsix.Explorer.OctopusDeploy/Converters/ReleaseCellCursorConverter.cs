// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReleaseCellCursorConverter.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
// Converter to return a cursor for a clickable release.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Services;

    using Octopus.Client.Model;

    /// <summary>
    /// Converter to return a cursor for a clickable release.
    /// </summary>
    /// <seealso cref="System.ComponentModel.TypeConverter" />
    internal class ReleaseCellCursorConverter : IValueConverter
    {
        /// <summary>
        /// Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        /// <param name="value">The source data being passed to the target.</param>
        /// <param name="targetType">The <see cref="T:System.Type"/> of data expected by the target dependency property.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="culture">The culture of the conversion.</param>
        /// <returns>
        /// The value to be passed to the target dependency property.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            return "Hand";
        }

        /// <summary>
        /// Modifies the target data before passing it to the source object.  This method is called only in <see cref="F:System.Windows.Data.BindingMode.TwoWay"/> bindings.
        /// </summary>
        /// <param name="value">The target data being passed to the source.</param>
        /// <param name="targetType">The <see cref="T:System.Type"/> of data expected by the source object.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="culture">The culture of the conversion.</param>
        /// <returns>
        /// The value to be passed to the source object.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
