using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace DeviceExplorer.Mvvm
{
    public class DialogView : Window
    {
        public DialogView()
        { }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            this.Owner = Application.Current.MainWindow;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            this.ShowInTaskbar = false;
        }

        #region DialogResult

        public static readonly DependencyProperty DialogResultProperty =
            DependencyProperty.RegisterAttached(
                "DialogResult",
                typeof(bool?),
                typeof(DialogView),
                new PropertyMetadata(DialogResultChanged));

        private static void DialogResultChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Window window = d as Window;
            if (window != null)
            {
                window.DialogResult = e.NewValue as bool?;
            }
        }

        public static void SetDialogResult(Window target, bool? value)
        {
            target.SetValue(DialogResultProperty, value);
        }

        /// <summary>
        /// Bind DialogResultProperty to DialogResult from DialogViewModel
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == DataContextProperty)
            {
                this.SetBinding(DialogResultProperty, "DialogResult");
            }

            base.OnPropertyChanged(e);
        }

        #endregion

        #region Window Styles

        const int GWL_STYLE = (-16);

        [Flags]
        private enum WindowStyles : uint
        {
            WS_SYSMENU = 0x80000,
            WS_MINIMIZEBOX = 0x20000,
            WS_MAXIMIZEBOX = 0x10000,
        }
        
        public const int SWP_FRAMECHANGED = 0x0020;
        public const int SWP_NOACTIVATE = 0x0010;
        public const int SWP_NOMOVE = 0x0002;
        public const int SWP_NOOWNERZORDER = 0x0200;
        public const int SWP_NOREPOSITION = 0x0200;
        public const int SWP_NOSIZE = 0x0001;
        public const int SWP_NOZORDER = 0x0004;

        
        private static void AddWindowStyle(Window window, WindowStyles styleToAdd)
        {
            try
            {
                WindowInteropHelper wih = new WindowInteropHelper(window);
                WindowStyles style = (WindowStyles)NativeMethods.GetWindowLong(wih.EnsureHandle(), GWL_STYLE);
                style |= styleToAdd;
                NativeMethods.SetWindowLong(wih.Handle, GWL_STYLE, (uint)style);
                NativeMethods.SetWindowPos(wih.Handle, IntPtr.Zero, 0, 0, 0, 0, SWP_FRAMECHANGED | SWP_NOACTIVATE | SWP_NOMOVE | SWP_NOOWNERZORDER | SWP_NOREPOSITION | SWP_NOSIZE | SWP_NOZORDER);

            }
            catch (Exception ex)
            {
                
                throw new Exception("AddWindowStyle failed", ex);
            }
        }

        private static void RemoveWindowStyle(Window window, WindowStyles styleToRemove)
        {
            try
            {
                WindowInteropHelper wih = new WindowInteropHelper(window);
                WindowStyles style = (WindowStyles) NativeMethods.GetWindowLong(wih.EnsureHandle(), GWL_STYLE);
                style &= ~styleToRemove;
                NativeMethods.SetWindowLong(wih.Handle, GWL_STYLE, (uint) style);
                NativeMethods.SetWindowPos(wih.Handle, IntPtr.Zero, 0, 0, 0, 0,
                    SWP_FRAMECHANGED | SWP_NOACTIVATE | SWP_NOMOVE | SWP_NOOWNERZORDER | SWP_NOREPOSITION | SWP_NOSIZE |
                    SWP_NOZORDER);
            }
            catch (Exception ex)
            {
                throw new Exception("RemoveWindowStyle failed", ex);
            }
        }

        public static bool GetShowMinimize(DependencyObject obj)
        {
            return (bool)obj.GetValue(ShowMinimizeProperty);
        }

        public static void SetShowMinimize(DependencyObject obj, bool value)
        {
            obj.SetValue(ShowMinimizeProperty, value);
        }

        public static readonly DependencyProperty ShowMinimizeProperty =
            DependencyProperty.RegisterAttached("ShowMinimize", typeof(bool), typeof(DialogView), new UIPropertyMetadata(true, OnShowMinimizeChanged));

        private static void OnShowMinimizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as Window;
            if (window == null)
            {
                if ((bool)e.NewValue)
                {
                    AddWindowStyle(window, WindowStyles.WS_MINIMIZEBOX);
                }
                else
                {
                    RemoveWindowStyle(window, WindowStyles.WS_MINIMIZEBOX);
                }
            }
        }

        public static bool GetShowMaximize(DependencyObject obj)
        {
            return (bool)obj.GetValue(ShowMaximizeProperty);
        }

        public static void SetShowMaximize(DependencyObject obj, bool value)
        {
            obj.SetValue(ShowMaximizeProperty, value);
        }

        public static readonly DependencyProperty ShowMaximizeProperty =
            DependencyProperty.RegisterAttached("ShowMaximize", typeof(bool), typeof(DialogView), new UIPropertyMetadata(true, OnShowMaximizeChanged));

        private static void OnShowMaximizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as Window;
            if (window != null)
            {
                if ((bool)e.NewValue)
                {
                    AddWindowStyle(window, WindowStyles.WS_MAXIMIZEBOX);
                }
                else
                {
                    RemoveWindowStyle(window, WindowStyles.WS_MAXIMIZEBOX);
                }
            }
        }
        
        public static bool GetShowHasSystemMenu(DependencyObject obj)
        {
            return (bool)obj.GetValue(ShowSystemMenuProperty);
        }

        public static void SetShowSystemMenu(DependencyObject obj, bool value)
        {
            obj.SetValue(ShowSystemMenuProperty, value);
        }

        public static readonly DependencyProperty ShowSystemMenuProperty =
            DependencyProperty.RegisterAttached("ShowSystemMenu", typeof(bool), typeof(DialogView), new UIPropertyMetadata(true, OnShowSystemMenuChanged));

        private static void OnShowSystemMenuChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as Window;
            if (window != null)
            {
                if ((bool)e.NewValue)
                {
                    AddWindowStyle(window, WindowStyles.WS_SYSMENU);
                }
                else
                {
                    RemoveWindowStyle(window, WindowStyles.WS_SYSMENU);
                }
            }
        } 

        public bool ShowSystemMenu
        {
            set { SetValue(ShowSystemMenuProperty, value); }
            get { return (bool)GetValue(ShowSystemMenuProperty); }
        }

        #endregion

        private static class NativeMethods
        {
            [DllImport("user32.dll")]
            internal static extern UInt32 GetWindowLong(IntPtr hWnd, int nIndex);

            [DllImport("user32.dll")]
            internal static extern int SetWindowLong(IntPtr hWnd, int nIndex, UInt32 dwNewLong);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);
        }
    }
}
