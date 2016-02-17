// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomDataGridTextColumn.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   The custom text column for the grid.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Infrastructure
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using DevOpsGuys.Vsix.Explorer.OctopusDeploy.Helpers;

    /// <summary>
    /// The custom text column for the grid.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.DataGridTextColumn" />
    internal class CustomDataGridTextColumn : DataGridTextColumn
    {
        /// <summary>
        /// Gets or sets the name of the template.
        /// </summary>
        /// <value>
        /// The name of the template.
        /// </value>
        public string TemplateName { get; set; }

        /// <summary>
        /// Gets a read-only <see cref="T:System.Windows.Controls.TextBlock" /> control that is bound to the column's <see cref="P:System.Windows.Controls.DataGridBoundColumn.Binding" /> property value.
        /// </summary>
        /// <param name="cell">The cell that will contain the generated element.</param>
        /// <param name="dataItem">The data item represented by the row that contains the intended cell.</param>
        /// <returns>
        /// A new, read-only text block control that is bound to the column's <see cref="P:System.Windows.Controls.DataGridBoundColumn.Binding" /> property value.
        /// </returns>
        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            Guard.ArgumentNotNull(cell, "cell");

            var binding = new Binding(((Binding)this.Binding).Path.Path) { Source = dataItem };
            var content = new ContentControl { ContentTemplate = (DataTemplate)cell.FindResource(this.TemplateName) };
            content.SetBinding(ContentControl.ContentProperty, binding);
            return content;
        }

        /// <summary>
        /// Gets a <see cref="T:System.Windows.Controls.TextBox" /> control that is bound to the column's <see cref="P:System.Windows.Controls.DataGridBoundColumn.Binding" /> property value.
        /// </summary>
        /// <param name="cell">The cell that will contain the generated element.</param>
        /// <param name="dataItem">The data item represented by the row that contains the intended cell.</param>
        /// <returns>
        /// A new text box control that is bound to the column's <see cref="P:System.Windows.Controls.DataGridBoundColumn.Binding" /> property value.
        /// </returns>
        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            return this.GenerateElement(cell, dataItem);
        }
    }
}
