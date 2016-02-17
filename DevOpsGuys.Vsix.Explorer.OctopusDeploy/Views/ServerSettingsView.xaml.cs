// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServerSettingsView.xaml.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   Interaction logic for ServerSettingsView.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Views
{
    using System.Windows;
    using System.Windows.Controls;

    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Helpers;

    /// <summary>
    /// Interaction logic for ServerSettingsView.
    /// </summary>
    public partial class ServerSettingsView
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="ServerSettingsView"/> class.
        /// </summary>
        public ServerSettingsView()
        {
            this.InitializeComponent();
            this.ApiKeyBox.PasswordChanged += this.ApiKeyChanged;
        }

        /// <summary>
        /// Handles the PasswordChanged event of the PasswordBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void ApiKeyChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).OctopusServerApiKey = ((PasswordBox)sender).SecurePassword.ToEncryptedString();
            }
        }
    }
}
