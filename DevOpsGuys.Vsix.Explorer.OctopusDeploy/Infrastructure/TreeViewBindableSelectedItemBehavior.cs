// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TreeViewBindableSelectedItemBehavior.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   A behaviour for making TreeView's SelectedItem bindable.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Infrastructure
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interactivity;
    using System.Windows.Media;

    /// <summary>
    /// A behaviour for making TreeView's SelectedItem bind.
    /// </summary>
    internal class TreeViewBindableSelectedItemBehavior : Behavior<TreeView>
    {
        /// <summary>
        /// The dependency property definition for the SelectedItem property.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible", Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed. Suppression is OK here.")]
        public static DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            "SelectedItem",
            typeof(object),
            typeof(TreeViewBindableSelectedItemBehavior),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnSelectedItemChanged));

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        public object SelectedItem
        {
            get { return this.GetValue(SelectedItemProperty); }
            set { this.SetValue(SelectedItemProperty, value); }
        }

        /// <summary>
        /// Called after the behaviour is attached to an AssociatedObject.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.SelectedItemChanged += this.OnSelectedItemChanged;
        }

        /// <summary>
        /// Called when the behaviour is being detached from its AssociatedObject, but before it has
        /// actually occurred.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (this.AssociatedObject != null)
            {
                this.AssociatedObject.SelectedItemChanged -= this.OnSelectedItemChanged;
            }
        }

        /// <summary>
        /// Called when the SelectedItem dependency property has changed.
        /// </summary>
        /// <param name="obj">The dependency object where the value has changed.</param>
        /// <param name="e">
        /// The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing
        /// the event data.
        /// </param>
        private static void OnSelectedItemChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var behavior = obj as TreeViewBindableSelectedItemBehavior;
            if (behavior != null)
            {
                var treeView = behavior.AssociatedObject;
                if (treeView != null)
                {
                    var treeViewItem = FindTreeViewItemRecursively(treeView, e.NewValue);
                    if (treeViewItem != null)
                    {
                        treeViewItem.SetValue(TreeViewItem.IsSelectedProperty, true);
                    }
                }
            }
        }

        /// <summary>
        /// Attempts to find a TreeViewItem recursively that is the container for the specified
        /// content to find.
        /// </summary>
        /// <remarks>
        /// Source: http://blogs.msdn.com/b/wpfsdk/archive/2010/02/23/finding-an-object-treeviewitem.aspx.
        /// </remarks>
        /// <param name="itemsControl">The items control.</param>
        /// <param name="contentToFind">The content to find.</param>
        /// <returns>The matching TreeViewItem, otherwise null.</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        private static TreeViewItem FindTreeViewItemRecursively(ItemsControl itemsControl, object contentToFind)
        {
            if (itemsControl == null)
            {
                return null;
            }

            if (itemsControl.DataContext == contentToFind)
            {
                return itemsControl as TreeViewItem;
            }

            ForceItemsControlToGenerateContainers(itemsControl);

            for (var i = 0; i < itemsControl.Items.Count; i++)
            {
                var childItemsControl = itemsControl.ItemContainerGenerator.ContainerFromIndex(i) as ItemsControl;
                var result = FindTreeViewItemRecursively(childItemsControl, contentToFind);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Forces the specified items control to generate containers for its items.
        /// </summary>
        /// <param name="itemsControl">The items control.</param>
        private static void ForceItemsControlToGenerateContainers(ItemsControl itemsControl)
        {
            itemsControl.ApplyTemplate();

            var itemsPresenter = (ItemsPresenter)itemsControl.Template.FindName("ItemsHost", itemsControl);

            if (itemsPresenter != null)
            {
                itemsPresenter.ApplyTemplate();
            }
            else
            {
                // The Tree template has not named the ItemsPresenter, so walk the descendents and
                // find the child.
                itemsPresenter = itemsControl.FindVisualChild<ItemsPresenter>();

                if (itemsPresenter == null)
                {
                    itemsControl.UpdateLayout();

                    itemsPresenter = itemsControl.FindVisualChild<ItemsPresenter>();
                }
            }

            if (itemsPresenter != null)
            {
                var itemsHostPanel = (Panel)VisualTreeHelper.GetChild(itemsPresenter, 0);

                // Ensure that the generator for this panel has been created.
                // ReSharper disable once UnusedVariable
                var children = itemsHostPanel.Children;
            }
        }

        /// <summary>
        /// Called when the SelectedItem has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">
        /// The <see cref="System.Windows.RoutedPropertyChangedEventArgs&lt;Object&gt;" /> instance
        /// containing the event data.
        /// </param>
        private void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            this.SelectedItem = e.NewValue;
        }
    }
}
