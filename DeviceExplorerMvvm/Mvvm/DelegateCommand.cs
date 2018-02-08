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
        //private List<WeakReference> canExecuteChangedHandlers;
               
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
        ///     Raises the CanExecuteChaged event
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        /// <summary>
        ///     Protected virtual method to raise CanExecuteChanged event
        /// </summary>
        //protected virtual void OnCanExecuteChanged()
        //{
        //    CommandManagerHelper.CallWeakReferenceHandlers(this.canExecuteChangedHandlers);
        //}
               

        /// <summary>
        /// ICommand.CanExecuteChanged implementation
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                //CommandManagerHelper.AddWeakReferenceHandler(ref this.canExecuteChangedHandlers, value, 2);
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
                //CommandManagerHelper.RemoveWeakReferenceHandler(this.canExecuteChangedHandlers, value);
            }
        }
    }
}