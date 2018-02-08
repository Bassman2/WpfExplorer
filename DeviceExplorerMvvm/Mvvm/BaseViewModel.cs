using Microsoft.Win32;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Threading;
using System.Xml.Serialization;

namespace DeviceExplorer.Mvvm
{
    public abstract class BaseViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        public BaseViewModel()
        { }

        #region Update

        /// <summary>
        /// Should be overwritten in derived classes to update the values;
        /// </summary>
        /// <returns>true if one ore more values are changed, else false</returns>
        public virtual bool OnUpdate()
        {
            return true;
        }
               
        #endregion
               
        #region INotifyPropertyChanged
        
        public event PropertyChangedEventHandler PropertyChanged;

        private delegate void NotifyPropertyChangedDeleagte(string propertyName);     

        public virtual void NotifyPropertyChanged(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }
            if (GetType().GetProperty(propertyName) == null)
            {
                throw new ArgumentOutOfRangeException(nameof(propertyName), "No property with name " + propertyName + " exists.");
            }

            if (Dispatcher.CurrentDispatcher.CheckAccess())
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            else
            {
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.DataBind, new NotifyPropertyChangedDeleagte(NotifyPropertyChanged), propertyName);
            }
        }

        protected void NotifyAllPropertiesChanged()
        {
            if (Dispatcher.CurrentDispatcher.CheckAccess())
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
            }
            else
            {
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.DataBind, new NotifyPropertyChangedDeleagte(NotifyPropertyChanged), null);
            }
        }

        #endregion

        #region IDataErrorInfo

        public virtual string Error
        {
            get
            {
                return string.Empty;
            }
        }

        public virtual string this[string columnName]
        {
            get
            {
                return string.Empty;
            }
        }

        #endregion
    }
}
