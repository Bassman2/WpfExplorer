using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace DeviceExplorer.Misc
{
    /*
    public sealed class MemoryData : IDisposable
    {
        private DispatcherTimer dispatcherTimer;
        private Func<DeviceInfo> func;
        private double max;

        public MemoryData()
        {
            this.MemoryValues = new ObservableCollection<double>();
            this.dispatcherTimer = new DispatcherTimer();
            this.dispatcherTimer.Tick += new EventHandler(OnTick);
           
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (func != null)
            {
                DeviceInfo deviceInfo = func();
                if (deviceInfo != null)
                {
                    if (deviceInfo.Total > 0)
                    {
                        double value = (double)deviceInfo.Used / deviceInfo.Total;
                        this.MemoryValues.Add(value);
                        this.FreeMemory = deviceInfo.Free;
                    }
                    else
                    {
                        this.max = Math.Max(this.max, deviceInfo.Free);
                    }
                }
            }
        }

        public void Start(int interval, Func<DeviceInfo> func)
        {
            this.func = func;
            this.dispatcherTimer.Interval = new TimeSpan(0, 0, interval);
            this.dispatcherTimer.Start();
        }

        public void Stop()
        {
            this.dispatcherTimer.Stop();
        }

        public void Reset()
        {
            this.MemoryValues.Clear();
        }

        public ObservableCollection<double> MemoryValues { get; private set; }
        public double FreeMemory { get; private set; }

        public void Dispose()
        {
            if (this.dispatcherTimer != null)
            {
                this.dispatcherTimer.Stop();
                this.dispatcherTimer = null;
            }
        }
    }
    */
}
