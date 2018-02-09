using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace DeviceExplorer.Mvvm
{
    /// <summary>
    ///     This class allows delegating the commanding logic to methods passed as parameters,
    ///     and enables a View to bind commands to objects that are not part of the element tree.
    /// </summary>
    public class DelegateCommand : ICommand
    {
        private readonly Action execute = null;
        private readonly Func<bool> canExecute = null;
               
        /// <summary>
        /// Constructor
        /// </summary>
        public DelegateCommand(Action execute, Func<bool> canExecute = null)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            this.execute = execute;
            this.canExecute = canExecute ?? new Func<bool>(() => true);
        }
        
        /// <summary>
        ///     Method to determine if the command can be executed
        /// </summary>
        public bool CanExecute(object param)
        {
            return this.canExecute();
        }

        /// <summary>
        ///     Execution of the command
        /// </summary>
        public void Execute(object param)
        {
            this.execute();
        }
        
        /// <summary>
        /// ICommand.CanExecuteChanged implementation
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }
    }
}