// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGridExtension.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   Extension to help bind a data matrix to a data grid.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Infrastructure
{
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Helpers;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Model;

    /// <summary>
    /// Extension to help bind a release matrix to a data grid.
    /// </summary>
    internal static class DataGridExtension
    {
        /// <summary>
        /// MatrixSource Attached Dependency Property.
        /// </summary>
        public static readonly DependencyProperty ReleaseMatrixSourceProperty =
            DependencyProperty.RegisterAttached(
                "ReleaseMatrixSource",
                typeof(ReleaseMatrixModel),
                typeof(DataGridExtension),
                new FrameworkPropertyMetadata(null, OnReleaseMatrixSourceChanged));

        /// <summary>
        /// Gets the matrix source.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <returns>A <see cref="ReleaseMatrixModel"/></returns>
        public static ReleaseMatrixModel GetReleaseMatrixSource(DependencyObject dependencyObject)
        {
            Guard.ArgumentNotNull(dependencyObject, "dependencyObject");
            return (ReleaseMatrixModel)dependencyObject.GetValue(ReleaseMatrixSourceProperty);
        }

        /// <summary>
        /// Sets the MatrixSource property.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="value">The value.</param>
        public static void SetReleaseMatrixSource(DependencyObject dependencyObject, ReleaseMatrixModel value)
        {
            Guard.ArgumentNotNull(dependencyObject, "dependencyObject");
            dependencyObject.SetValue(ReleaseMatrixSourceProperty, value);
        }

        /// <summary>
        /// Handles changes to the MatrixSource property.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="eventArgs">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnReleaseMatrixSourceChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            var grid = dependencyObject as DataGrid;
            var releaseMatrix = eventArgs.NewValue as ReleaseMatrixModel;
            if (grid != null)
            {
                grid.ItemsSource = releaseMatrix;
                grid.Columns.Clear();
                if (releaseMatrix != null)
                {
                    foreach (var col in releaseMatrix.Environments)
                    {
                        grid.Columns.Add(
                            new CustomDataGridTextColumn
                                {
                                    Header = col.Name,
                                    Binding = new Binding(string.Format(CultureInfo.InvariantCulture, "[{0}]", col.Index)),
                                    TemplateName = "DynamicCellBackgroundTemplate",
                                    Width = new DataGridLength(1, DataGridLengthUnitType.Star)
                            });
                    }
                }
            }
        }
    }
}