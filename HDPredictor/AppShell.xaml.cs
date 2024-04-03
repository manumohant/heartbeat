using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace HDPredictor
{
    public partial class AppShell : Shell
    {
        
        public static event PropertyChangedEventHandler DeviceConnected;
        public static event PropertyChangedEventHandler DeviceDataReceived;
        public static event PropertyChangedEventHandler DeviceError;
        public static event PropertyChangedEventHandler DeviceLog;
        public AppShell()
        {
            InitializeComponent(); 
        }
        public static void BTDevices(string name)
        { 
            DeviceConnected?.Invoke(null, new PropertyChangedEventArgs(name));
        }
        public static void BTDataReceived(string data)
        {
            DeviceDataReceived?.Invoke(null, new PropertyChangedEventArgs(data));
        }
        public static void BTError(string error)
        {
            DeviceError?.Invoke(null, new PropertyChangedEventArgs(error));
        }
        public static void BTLog(string logs)
        {
            DeviceLog?.Invoke(null, new PropertyChangedEventArgs(logs));
        }
    }
}
