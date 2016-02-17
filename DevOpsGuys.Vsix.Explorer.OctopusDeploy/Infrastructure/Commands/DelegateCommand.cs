// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DelegateCommand.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   A basic ICommand implementation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.UI
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// A basic ICommand implementation.
    /// </summary>
    internal class DelegateCommand : ICommand
    {
        /// <summary>
        /// The can execute check.
        /// </summary>
        private readonly Predicate<object> canExecute;

        /// <summary>
        /// The execute action.
        /// </summary>
        private readonly Action<object> execute;

        /// <summary>
        /// Initialises a new instance of the <see cref="DelegateCommand" /> class.
        /// </summary>
        /// <param name="execute">The execute handler.</param>
        /// <param name="canExecute">The can execute handler.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Reviewed. Suppression is OK here.")]
        public DelegateCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command. If the command does not require data to be passed, this object
        /// can be set to null.
        /// </param>
        /// <returns>True if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command. If the command does not require data to be passed, this object
        /// can be set to null.
        /// </param>
        public void Execute(object parameter)
        {
            this.execute(parameter);
        }

        /// <summary>
        /// Raises the can execute changed.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate", Justification = "Reviewed. Suppression is OK here.")]
        public void RaiseCanExecuteChanged()
        {
            if (this.CanExecuteChanged != null)
            {
                this.CanExecuteChanged.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
