// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExplorerViewModel.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   ViewModel for main explorer window.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.Windows;

    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Infrastructure;
    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.UI;

    /// <summary>
    /// ViewModel for main explorer window.
    /// </summary>
    internal class ExplorerViewModel : Bindable
    {
        /// <summary>
        /// Gets or sets the octopus deploy model.
        /// </summary>
        /// <value>
        /// The octopus deploy model.
        /// </value>
        public OctopusViewModel OctopusViewModel
        {
            get
            {
                var item = this.GetPropertyValue<OctopusViewModel>();
                if (item != null)
                {
                    this.HasException = string.IsNullOrWhiteSpace(item.ExceptionMessage) ? Visibility.Hidden : Visibility.Visible;
                    this.HasExceptionResolution = string.IsNullOrWhiteSpace(item.ExceptionMessageButtonText) ? Visibility.Hidden : Visibility.Visible;
                    this.IsTreeVisible = string.IsNullOrWhiteSpace(item.ExceptionMessage) ? Visibility.Visible : Visibility.Hidden;
                }
                
                return item;
            }

            set
            {
                this.SetPropertyValue(value);
                if (value != null)
                {
                    this.HasException = string.IsNullOrWhiteSpace(value.ExceptionMessage) ? Visibility.Hidden : Visibility.Visible;
                    this.HasExceptionResolution = string.IsNullOrWhiteSpace(value.ExceptionMessageButtonText) ? Visibility.Hidden : Visibility.Visible;
                    this.IsTreeVisible = string.IsNullOrWhiteSpace(value.ExceptionMessage) ? Visibility.Visible : Visibility.Hidden;
                }
            }
        }

        /// <summary>
        /// Gets the semantic version number.
        /// </summary>
        /// <value>
        /// The semantic version number.
        /// </value>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Reviewed")]
        public string SemanticVersionNumber
        {
            get
            {
                var assembley = Assembly.GetExecutingAssembly();
                var assemblyName = assembley.GetName().Name;
                var gitVersionInformationType = assembley.GetType(assemblyName + ".GitVersionInformation");
                return gitVersionInformationType.GetField("SemVer").GetValue(null).ToString();
            }
        }

        /// <summary>
        /// Gets or sets the has exception.
        /// </summary>
        /// <value>
        /// The has exception.
        /// </value>
        public Visibility HasException
        {
            get { return this.GetPropertyValue<Visibility>(); }
            set { this.SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the has exception resolution.
        /// </summary>
        /// <value>
        /// The has exception resolution.
        /// </value>
        public Visibility HasExceptionResolution
        {
            get { return this.GetPropertyValue<Visibility>(); }
            set { this.SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the is tree visible.
        /// </summary>
        /// <value>
        /// The is tree visible.
        /// </value>
        public Visibility IsTreeVisible
        {
            get { return this.GetPropertyValue<Visibility>(); }
            set { this.SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this grid is visible.
        /// </summary>
        /// <value>
        /// <c>True</c> if this grid is visible; otherwise, <c>false</c>.
        /// </value>
        public Visibility IsGridVisible
        {
            get { return this.GetPropertyValue<Visibility>(); }
            set { this.SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether items are refreshing.
        /// </summary>
        public bool IsRefreshing
        {
            get { return this.GetPropertyValue<bool>(); }
            set { this.SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the name filter.
        /// </summary>
        public string NameFilter
        {
            get { return this.GetPropertyValue<string>(); }
            set { this.SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the selected project item.
        /// </summary>
        /// <value>
        /// The selected project item.
        /// </value>
        public OctopusProjectViewModel SelectedProjectItem
        {
            get
            {
                var item = this.GetPropertyValue<OctopusProjectViewModel>();
                this.IsGridVisible = item != null ? Visibility.Visible : Visibility.Hidden;
                return item;
            }

            set
            {
                this.SetPropertyValue(value);
                this.IsGridVisible = value != null ? Visibility.Visible : Visibility.Hidden;
            }
        }
    }
}
