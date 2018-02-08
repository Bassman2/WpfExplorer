using DeviceExplorer.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceExplorer.ViewModel
{
    public class CreateFolderViewModel : DialogViewModel
    {
        private string folderName;

        public string FolderName
        {
            get { return this.folderName; }
            set { this.folderName = value; NotifyPropertyChanged(nameof(FolderName)); }
        }
    }
}
