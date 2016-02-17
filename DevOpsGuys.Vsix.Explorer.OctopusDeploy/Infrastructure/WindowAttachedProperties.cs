// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowAttachedProperties.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   A helper class for attached properties on <see cref="Window" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Infrastructure
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;

    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Helpers;

    /// <summary>
    /// A helper class for attached properties on <see cref="Window" />.
    /// </summary>
    internal static class WindowAttachedProperties
    {
        /// <summary>
        /// The dependency property definition for the DialogResult attached property.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible", Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed. Suppression is OK here.")]
        public static DependencyProperty DialogResultProperty = 
            DependencyProperty.RegisterAttached(
            "DialogResult",
            typeof(bool?),
            typeof(WindowAttachedProperties),
            new FrameworkPropertyMetadata(OnDialogResultChanged));

        /// <summary>
        /// Gets the DialogResult value from the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns>The value.</returns>
        public static bool? GetDialogResult(DependencyObject target)
        {
            Guard.ArgumentNotNull(target, "target");
            return (bool?)target.GetValue(DialogResultProperty);
        }

        /// <summary>
        /// Sets the DialogResult value on the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="value">The value.</param>
        public static void SetDialogResult(DependencyObject target, bool? value)
        {
            Guard.ArgumentNotNull(target, "target");
            target.SetValue(DialogResultProperty, value);
        }

        /// <summary>
        /// Called when the DialogResult attached property has changed.
        /// </summary>
        /// <param name="obj">The dependency object where the value has changed.</param>
        /// <param name="e">
        /// The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing
        /// the event data.
        /// </param>
        private static void OnDialogResultChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var window = obj as Window;
            if (window != null)
            {
                window.DialogResult = e.NewValue as bool?;
            }
        }
    }
}
