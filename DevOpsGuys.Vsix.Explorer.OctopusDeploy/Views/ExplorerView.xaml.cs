// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExplorerView.xaml.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   Interaction logic for ExplorerPaneControl.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Views
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Controls;

    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Converters;

    /// <summary>
    /// Interaction logic for ExplorerPaneControl.
    /// </summary>
    public partial class ExplorerView : UserControl
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="ExplorerView"/> class.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "To Review")]
        public ExplorerView()
        {
            this.InitializeComponent();

            // Use this as a double click
            this.dataGrid.BeginningEdit += (sender, args) =>
                {
                    try
                    {
                        var link = new ReleaseLinkConverter().Convert(((ContentControl)args.EditingEventArgs.Source).Content, null, null, null);
                        if (link != null)
                        {
                            Process.Start(link.ToString());
                        }
                    }
                    catch (Exception)
                    {
                    }

                    args.Cancel = true;
                };
        }
    }
}