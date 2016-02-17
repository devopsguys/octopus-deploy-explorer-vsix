// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TreeViewItemViewModel.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   Base class for all ViewModel classes displayed by TreeViewItems.
//   This acts as an adapter between a raw data object and a TreeViewItem.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows;

    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Infrastructure;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.UI;

    /// <summary>
    /// Base class for all ViewModel classes displayed by TreeViewItems.  
    /// This acts as an adapter between a raw data object and a TreeViewItem.
    /// </summary>
    internal class TreeViewItemViewModel : Bindable
    {
        /// <summary>
        /// The dummy child.
        /// </summary>
        private static readonly TreeViewItemViewModel DummyChild = new TreeViewItemViewModel();
        
        /// <summary>
        /// The children.
        /// </summary>
        private readonly ObservableCollection<TreeViewItemViewModel> children;

        /// <summary>
        /// The parent.
        /// </summary>
        private readonly TreeViewItemViewModel parent;

        /// <summary>
        /// Initialises a new instance of the <see cref="TreeViewItemViewModel"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        protected TreeViewItemViewModel(TreeViewItemViewModel parent)
        {
            this.parent = parent;
            this.children = new ObservableCollection<TreeViewItemViewModel> { DummyChild };
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="TreeViewItemViewModel"/> class from being created.
        /// </summary>
        private TreeViewItemViewModel()
        {
        }

        /// <summary>
        /// Gets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public TreeViewItemViewModel Parent
        {
            get
            {
                return this.parent;
            }
        }

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <value>
        /// The children.
        /// </value>
        public ObservableCollection<TreeViewItemViewModel> Children
        {
            get
            {
                return this.children;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has dummy child.
        /// </summary>
        /// <value>
        /// <c>True</c> if this instance has dummy child; otherwise, <c>false</c>.
        /// </value>
        public bool HasDummyChild
        {
            get
            {
                return this.Children.Count == 1 && this.Children[0] == DummyChild;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is expanded.
        /// </summary>
        /// <value>
        /// <c>True</c> if this instance is expanded; otherwise, <c>false</c>.
        /// </value>
        public bool IsExpanded
        {
            get
            {
                return this.GetPropertyValue<bool>();
            }

            set
            {
                this.SetPropertyValue(value);

                // Expand all the way up to the root.
                if (this.GetPropertyValue<bool>() && this.Parent != null)
                {
                    this.Parent.IsExpanded = true;
                }

                // Lazy load the child items, if necessary.
                if (this.HasDummyChild)
                {
                    this.Children.Remove(DummyChild);
                    this.LoadChildren();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is selected.
        /// </summary>
        /// <value>
        /// <c>True</c> if this instance is selected; otherwise, <c>false</c>.
        /// </value>
        public bool IsSelected
        {
            get
            {
                return this.GetPropertyValue<bool>();
            }

            set
            {
                if (this.GetPropertyValue<bool>() == value)
                {
                    return;
                }

                this.SetPropertyValue(value);

                if (value)
                {
                    this.LoadSelected();
                }
            }
        }

        /// <summary>
        /// Gets or sets the is visible.
        /// </summary>
        /// <value>
        /// The is visible.
        /// </value>
        public Visibility IsVisible
        {
            get { return this.GetPropertyValue<Visibility>(); }
            set { this.SetPropertyValue(value); }
        }

        /// <summary>
        /// Applies the filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        public void ApplyFilter(string filter)
        {
            if (this.IsFilterMatched(filter))
            {
                this.IsVisible = Visibility.Visible;

                if (this.Parent != null)
                {
                    this.Parent.IsVisible = Visibility.Visible;
                }
            }
            else
            {
                this.IsVisible = Visibility.Collapsed;
            }

            foreach (var child in this.Children)
            {
                if (child.GetType() != typeof(TreeViewItemViewModel))
                {
                    child.ApplyFilter(filter);
                }
            }
        }

        /// <summary>
        /// Invoked when the child items need to be loaded on demand.
        /// Subclasses can override this to populate the Children collection.
        /// </summary>
        protected virtual void LoadChildren()
        {
        }

        /// <summary>
        /// Invoked when the selected item need to be loaded on demand.
        /// </summary>
        protected virtual void LoadSelected()
        {
        }

        /// <summary>
        /// Determines whether [is filter matched] [the filter criteria].
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>
        /// A flag indicating a match.
        /// </returns>
        protected virtual bool IsFilterMatched(string filter)
        {
            return true;
        }
    }
}
