using DeviceExplorer.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceExplorer.ViewModel
{
    public class CreateLinkViewModel : DialogViewModel
    {
        private string linkName;
        private string linkPath;

        public string LinkName
        {
            get { return this.linkName; }
            set { this.linkName = value; NotifyPropertyChanged(nameof(LinkName)); }
        }

        public string LinkPath
        {
            get { return this.linkPath; }
            set { this.linkPath = value; NotifyPropertyChanged(nameof(LinkPath)); }
        }
        
    }
}
