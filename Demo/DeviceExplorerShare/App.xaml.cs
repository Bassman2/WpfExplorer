using System;
using System.Diagnostics;
using System.Windows;

namespace DeviceExplorer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            try
            {
                AppDomain.CurrentDomain.UnhandledException += (s, a) =>
                {
                    Exception ex = (Exception)a.ExceptionObject;
                    Trace.TraceError(ex.ToString());
                    MessageBox.Show(ex.ToString(), "Unhandled Error !!!");
                };

                Trace.TraceInformation($"Startup {DateTime.Now.ToLocalTime().ToShortTimeString()} {DateTime.Now.ToLocalTime().ToShortDateString()}");

                new MainWindow().Show();
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
