using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DeviceExplorer.Mvvm
{
    /// <summary>
    /// View Model base for dialogs based on DialogView and DialogButtonsView
    /// </summary>
    public abstract class DialogViewModel : BaseViewModel
    {
        private bool? dialogResult = null;
        
        /// <summary>
        /// DelegateCommand for the OK button 
        /// </summary>
        public DelegateCommand OKCommand { get; private set; }

        /// <summary>
        /// DelegateCommand for the Cancel button 
        /// </summary>
        public DelegateCommand CancelCommand { get; private set; }

        /// <summary>
        /// DelegateCommand for the Close button 
        /// </summary>
        public DelegateCommand CloseCommand { get; private set; }
                        
        /// <summary>
        /// Constructor
        /// </summary>
        public DialogViewModel()
        {
            this.OKCommand = new DelegateCommand(OnOK);
            this.CancelCommand = new DelegateCommand(OnCancel);
            this.CloseCommand = new DelegateCommand(OnClose);
        }
        
        /// <summary>
        /// true if the OK button was pressed, else false
        /// </summary>
        public bool? DialogResult
        {
            get
            {
                return this.dialogResult;
            }
            set
            {
                this.dialogResult = value;
                NotifyPropertyChanged(nameof(DialogResult));
            }
        }
        
        /// <summary>
        /// Handler for the OK button
        /// </summary>
        protected virtual void OnOK()
        {
            // update current focused element
            var focusedElement = Keyboard.FocusedElement as FrameworkElement;
            if (focusedElement is TextBox)
            {
                var expression = focusedElement.GetBindingExpression(TextBox.TextProperty);
                if (expression != null)
                {
                    expression.UpdateSource();
                }
            }

            this.DialogResult = OnUpdate();
        }

        /// <summary>
        /// Handler for the Cancel button
        /// </summary>
        protected virtual void OnCancel()
        {
            this.DialogResult = false;
        }

        /// <summary>
        /// Handler for the Close button
        /// </summary>
        protected void OnClose()
        {
            if (this.dialogResult == null)
            {
                OnCancel();
            }
        }
    }
}
