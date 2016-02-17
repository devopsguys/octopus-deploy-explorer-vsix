// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DependencyObjectExtensions.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   A set of extension methods for <see cref="DependencyObject" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Infrastructure
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Media3D;

    /// <summary>
    /// A set of extension methods for <see cref="DependencyObject" />.
    /// </summary>
    internal static class DependencyObjectExtensions
    {
        /// <summary>
        /// Attempts to find the closest visual ancestor of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the ancestor.</typeparam>
        /// <param name="obj">The object to search.</param>
        /// <returns>The closest matching visual ancestor, otherwise null.</returns>
        public static T FindVisualAncestor<T>(this DependencyObject obj)
            where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(obj.FindVisualTreeRoot());

            while (parent != null)
            {
                var ancestor = parent as T;
                if (ancestor != null)
                {
                    return ancestor;
                }

                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
        }

        /// <summary>
        /// Attempts to find a visual child of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the child.</typeparam>
        /// <param name="obj">The object to search.</param>
        /// <returns>A matching visual child, otherwise null.</returns>
        public static T FindVisualChild<T>(this DependencyObject obj)
            where T : DependencyObject
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                var visualChild = child as T;
                if (visualChild != null)
                {
                    return visualChild;
                }

                var descendant = FindVisualChild<T>(child);
                if (descendant != null)
                {
                    return descendant;
                }
            }

            return null;
        }

        /// <summary>
        /// Attempts to find all visual children of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the child.</typeparam>
        /// <param name="obj">The object to search.</param>
        /// <returns>The matching visual children, may be null.</returns>
        public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject obj)
            where T : DependencyObject
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                var children = child as T;
                if (children != null)
                {
                    yield return children;
                }

                var descendants = FindVisualChildren<T>(child);
                foreach (var descendant in descendants)
                {
                    yield return descendant;
                }
            }
        }

        /// <summary>
        /// Attempts to find the closest visual tree root, working with the logical tree hierarchy.
        /// </summary>
        /// <param name="obj">The object to search.</param>
        /// <returns>A matching visual ancestor, otherwise null.</returns>
        public static DependencyObject FindVisualTreeRoot(this DependencyObject obj)
        {
            var current = obj;
            var result = obj;

            while (current != null)
            {
                result = current;
                if (current is Visual || current is Visual3D)
                {
                    break;
                }

                // If the current item is not a visual, try to walk up the logical tree.
                current = LogicalTreeHelper.GetParent(current);
            }

            return result;
        }
    }
}
