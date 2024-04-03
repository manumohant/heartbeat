
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs; 

namespace HDPredictor
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        public ObservableCollection<IDevice> devices { get; set; } 
         

        public MainPage()
        {
            devices = new ObservableCollection<IDevice>();
            InitializeComponent();
            BindingContext = this;  
            var adapter = CrossBluetoothLE.Current.Adapter;
            Plugin.BLE.CrossBluetoothLE.Current.StateChanged += Current_StateChanged;
            adapter.DeviceDiscovered += Adapter_DeviceDiscovered;  

            adapter.StartScanningForDevicesAsync();

            foreach(var device in adapter.BondedDevices)
            {
                if(!devices.Any(p=>p.Id == device.Id))
                    devices.Add(device);
            }

        }

        private void Adapter_DeviceDiscovered(object? sender, DeviceEventArgs e)
        {
            if(e.Device!=null && e.Device.Name!=null && !devices.Any(p => p.Id == e.Device.Id))
                devices.Add(e.Device);
        }         


        private async void OnConnectClicked(object sender, EventArgs e)
        {
            var _bluetoothAdapter = CrossBluetoothLE.Current;
            
            if (!_bluetoothAdapter.IsAvailable || !_bluetoothAdapter.IsOn)
            {
                // Bluetooth is not available or not enabled
                return;
            }

            

            var selectedDevice = this.devicePicker.SelectedItem  as IDevice;

            var _device = await _bluetoothAdapter.Adapter.ConnectToKnownDeviceAsync(selectedDevice.Id);

            

            var services = await _device.GetServicesAsync();


            foreach(var service in services)
            {


                var _characteristics = await service.GetCharacteristicsAsync();
                foreach (var character in _characteristics)
                {
                    try
                    {
                        character.ValueUpdated += _characteristic_ValueUpdated; ;

                        await character.StartUpdatesAsync();
                    }
                    catch(Exception ex) 
                    {
                    }
                }
            }

            
        }

        private void _characteristic_ValueUpdated(object? sender, CharacteristicUpdatedEventArgs e)
        { 

            MainThread.BeginInvokeOnMainThread(() =>
            {
                this.logs.Text += $"Data received: {e.Characteristic.Name}=" + e.Characteristic.StringValue+"\r\n";
                this.HeartBeatEntry.Text = e.Characteristic.StringValue;
            });
            

        }

        private void Current_StateChanged(object? sender, Plugin.BLE.Abstractions.EventArgs.BluetoothStateChangedArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                this.logs.Text += $"Bluetooth status: {e.NewState} " + "\r\n";
            });
            if(e.NewState == BluetoothState.Off)
            {
                this.devices.Clear();
            }
            else if(e.NewState == BluetoothState.On) 
            {
                var adapter = CrossBluetoothLE.Current.Adapter;
                adapter.StartScanningForDevicesAsync();

                foreach (var device in adapter.BondedDevices)
                {
                    if (!devices.Any(p => p.Id == device.Id))
                        devices.Add(device);
                }
            }
        }

        private void OptionsPicker_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Submit_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Sent", "Data sent for processing. Please wait for the result","Close");
        }
    }

}
