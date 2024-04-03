
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks; 

namespace HDPredictor
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        public ObservableCollection<string> devices { get; set; }
        public static event PropertyChangedEventHandler MainPageLoaded;
        public static event PropertyChangedEventHandler MainPageDeviceConnectRequest;

        public MainPage()
        {
            devices = new ObservableCollection<string>();
            InitializeComponent();
            BindingContext = this;
            AppShell.DeviceConnected += AppShell_DeviceConnected;
            AppShell.DeviceDataReceived += AppShell_DeviceDataReceived;
            AppShell.DeviceError += AppShell_DeviceError;
            AppShell.DeviceLog += AppShell_DeviceLog;
            MainPageLoaded?.Invoke(this,new PropertyChangedEventArgs(nameof(MainPage))); 
        }

        private async void AppShell_DeviceLog(object? sender, PropertyChangedEventArgs e)
        {
            this.logs.Text += "Log: "+e.PropertyName +"\r\n";
        }

        private async void AppShell_DeviceError(object? sender, PropertyChangedEventArgs e)
        {
            this.logs.Text += "Error: "+e.PropertyName + "\r\n";
        }

        private async void AppShell_DeviceDataReceived(object? sender, PropertyChangedEventArgs e)
        {
            this.logs.Text += "Data received: "+ e.PropertyName + "\r\n";
        }

        private void AppShell_DeviceConnected(object? sender, PropertyChangedEventArgs e)
        { 
            if(devices.IndexOf(e.PropertyName) == -1)
            {
                devices.Add(e.PropertyName);
                this.OnPropertyChanged("devices");
            }
        }

        private async void OnConnectClicked(object sender, EventArgs e)
        {
            var data = this.devicePicker.SelectedItem as string;

            MainPageDeviceConnectRequest?.Invoke(this, new PropertyChangedEventArgs(data));
        }

        private void OptionsPicker_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

}
