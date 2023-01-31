using System.Windows;

namespace ExplorerCtrl.Internal
{
    /// <summary>
    /// Interaction logic for ProgresshWindow.xaml
    /// </summary>
    internal partial class ProgresshWindow : Window
    {
        public ProgresshWindow()
        {
            InitializeComponent();
        }

        public void Update(double percentage, string file = null)
        {
            this.progressBar.Value = percentage;
            if (file != null)
            {
                this.currentFile.Text = file;
            }
        }

        public bool IsCancelPendíng { get; private set; }
        
        private void OnCancel(object sender, RoutedEventArgs e)
        {
            this.IsCancelPendíng = true;
        }
    }
}
