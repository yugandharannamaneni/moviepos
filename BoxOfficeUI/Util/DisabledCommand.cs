﻿using FirstFloor.ModernUI.Presentation;
using System;

namespace BoxOfficeUI
{
    /// <summary>
    /// An ICommand implementation that cannot execute.
    /// </summary>
    public class DisabledCommand
        : CommandBase
    {
        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        public override bool CanExecute(object parameter)
        {
            return false; // cannot execute
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        protected override void OnExecute(object parameter)
        {
            throw new NotSupportedException();
        }
    }
}
