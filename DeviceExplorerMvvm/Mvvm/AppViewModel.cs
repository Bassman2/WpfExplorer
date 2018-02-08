using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shell;
using Microsoft.Win32;
using System.Reflection;
using System.ComponentModel;
using System.Windows.Input;
using System.Configuration;
using System.Diagnostics;
using System.Windows.Threading;

namespace DeviceExplorer.Mvvm
{
    public abstract class AppViewModel : BaseViewModel
    {
        private TaskbarItemProgressState progressState = TaskbarItemProgressState.None;
        private double progressValue = 0.0;
        private string statusText = "Ready";
        private string progressTime = string.Empty;

        public DelegateCommand StartupCommand { get; private set; }
        public DelegateCommand RefreshCommand { get; private set; }
        public DelegateCommand ImportCommand { get; private set; }
        public DelegateCommand ExportCommand { get; private set; }
        public DelegateCommand UndoCommand { get; private set; }
        public DelegateCommand RedoCommand { get; private set; }
        public DelegateCommand OptionsCommand { get; private set; }
        public DelegateCommand AboutCommand { get; private set; }
        public DelegateCommand HelpCommand { get; private set; }
        public DelegateCommand ExitCommand { get; private set; }
        public DelegateCommand<CancelEventArgs> ClosingCommand { get; set; }

        public AppViewModel()
        {
            this.StartupCommand = new DelegateCommand(OnStartup);
            this.RefreshCommand = new DelegateCommand(OnRefresh, OnCanRefresh);
            this.ImportCommand = new DelegateCommand(OnImport, OnCanImport);
            this.ExportCommand = new DelegateCommand(OnExport, OnCanExport);
            this.UndoCommand = new DelegateCommand(OnUndo, OnCanUndo);
            this.RedoCommand = new DelegateCommand(OnRedo, OnCanRedo);
            this.OptionsCommand = new DelegateCommand(OnOptions, OnCanOptions);
            this.AboutCommand = new DelegateCommand(OnAbout);
            this.HelpCommand = new DelegateCommand(OnHelp);
            this.ExitCommand = new DelegateCommand(OnExit, OnCanExit);
            this.ClosingCommand = new DelegateCommand<CancelEventArgs>(OnClosing);

            //BaseEngine.ProgressState += OnProgressState;
            //BaseEngine.ProgressValue += OnProgressValue;
            //BaseEngine.ProgressText += OnProgressText;
        }

        
        
        //private void OnProgressState(ProgressStateEventArgs args)
        //{
        //    this.ProgressState = args.State;
        //}

        //private void OnProgressValue(ProgressValueEventArgs args)
        //{
        //    this.ProgressValue = args.Value;
        //    this.ProgressTime = args.ElapsedTime.HasValue ?  $"{args.ElapsedTime:h':'mm':'ss} / {args.RemainingTime:h':'mm':'ss}" : "";
        //}

        //private void OnProgressText(ProgressTextEventArgs args)
        //{
        //    this.StatusText = args.Text;
        //}

        #region properties
        
        /// <summary>
        /// The main title of the application. Displayed in the main window header and in the taskbar.
        /// </summary>
        public virtual string Title
        {
            get
            {
                Assembly app = Assembly.GetEntryAssembly();
                string title = app.GetCustomAttribute<AssemblyTitleAttribute>().Title;
                Version ver = app.GetName().Version;
                string version = ver.Build > 0 ? ver.ToString(3) : ver.ToString(2);
                return $"{title} {version}";
            }
        }
        
        /// <summary>
        /// Progress state of the application. Displayed in the taskbar and in the statusbar.
        /// </summary>
        public TaskbarItemProgressState ProgressState
        {
            get
            {
                return this.progressState;
            }
            set
            {
                if (this.progressState != value)
                {
                    this.progressState = value;
                    NotifyPropertyChanged(nameof(ProgressState));
                }
            }
        }
        
        /// <summary>
        /// Progress value of the application. Displayed in the taskbar and in the statusbar.
        /// </summary>
        /// <remarks>
        /// The progress value is only visible if the <see cref="ProgressState"/> is not None and is in the range between 0.0 and 1.0.
        /// </remarks>
        public double ProgressValue
        {
            get
            {
                return this.progressValue;
            }
            set
            {
                if (this.progressValue != value)
                {
                    this.progressValue = value;
                    NotifyPropertyChanged(nameof(ProgressValue));

                    this.ProgressState = this.progressValue < 0.0 ? TaskbarItemProgressState.None : TaskbarItemProgressState.Normal;
                }
            }
        }

        public string ProgressTime
        {
            get
            {
                return this.progressTime;
            }
            set
            {
                if (this.progressTime != value)
                {
                    this.progressTime = value;
                    NotifyPropertyChanged(nameof(ProgressTime));
                }
            }
        }

        /// <summary>
        /// Status text in status line.
        /// </summary>
        public string StatusText
        {
            get
            {
                return this.statusText;
            }
            set
            {
                this.statusText = value;
                NotifyPropertyChanged(nameof(StatusText));
            }
        }

        #endregion

        #region command methods

        public virtual void OnStartup()
        {
            if (Application.Current == null)
            {
                // for testing
                OnActivate();
            }
            else
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => OnActivate()), DispatcherPriority.ContextIdle, null);
            }
        }

        protected virtual void OnActivate()
        { }

        protected virtual bool OnCanRefresh()
        {
            return true;
        }

        protected virtual void OnRefresh()
        { }

        protected virtual void OnImport()
        { }

        protected virtual bool OnCanImport()
        {
            return this.ProgressState == TaskbarItemProgressState.None;
        }

        protected virtual void OnExport()
        { }
        
        protected virtual bool OnCanExport()
        {
            return this.ProgressState == TaskbarItemProgressState.None;
        }

        protected virtual void OnUndo()
        { }
        
        protected virtual bool OnCanUndo()
        {
            return false;
        }

        protected virtual void OnRedo()
        { }

        protected virtual bool OnCanRedo()
        {
            return false;
        }

        protected virtual void OnOptions()
        { }

        protected virtual bool OnCanOptions()
        {
            return true;
        }

        protected virtual void OnAbout()
        { }

        protected virtual void OnHelp()
        {
            string path = Path.ChangeExtension(Assembly.GetEntryAssembly().Location, ".chm");
            if (File.Exists(path))
            {
                System.Diagnostics.Process.Start(path);
            }
            else
            {
                MessageBox.Show(string.Format("Help file \"{0}\" not found!", path), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected virtual void OnExit()
        {
            Application.Current.MainWindow.Close();
        }
        
        protected virtual bool OnCanExit()
        {
            return true;
        }
        
        private void OnClosing(CancelEventArgs e)
        {
            if (e != null)
            {
                e.Cancel = !OnClosing();
            }
        }

        protected virtual bool OnClosing()
        {
            return true;
        }

        #endregion
    }
}
