using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace HDPredictor;

public partial class POMPage : ContentPage
{
    public ObservableCollection<IDevice> devices { get; set; }
    private Dictionary<string, byte[]> bleData = new Dictionary<string, byte[]>(); 
    public POMPage()
	{
        devices = new ObservableCollection<IDevice>();
        InitializeComponent();
        BindingContext = this;
        var adapter = CrossBluetoothLE.Current.Adapter;
        Plugin.BLE.CrossBluetoothLE.Current.StateChanged += Current_StateChanged;
        adapter.DeviceDiscovered += Adapter_DeviceDiscovered;

        adapter.StartScanningForDevicesAsync();

        foreach (var device in adapter.BondedDevices)
        {
            if (!devices.Any(p => p.Id == device.Id))
                devices.Add(device);
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
		await Navigation.PushAsync(new ECGPage(), true);
    }
    private async void OnConnectClicked(object sender, EventArgs e)
    {
        var _bluetoothAdapter = CrossBluetoothLE.Current;

        if (!_bluetoothAdapter.IsAvailable || !_bluetoothAdapter.IsOn)
        {
            // Bluetooth is not available or not enabled
            return;
        }

        if (this.devicePicker.SelectedItem == null) return;

        var selectedDevice = this.devicePicker.SelectedItem as IDevice;

        var _device = await _bluetoothAdapter.Adapter.ConnectToKnownDeviceAsync(selectedDevice.Id);



        var services = await _device.GetServicesAsync();


        foreach (var service in services)
        {


            var _characteristics = await service.GetCharacteristicsAsync();
            foreach (var character in _characteristics)
            {
                try
                {
                    character.ValueUpdated += _characteristic_ValueUpdated; ;

                    await character.StartUpdatesAsync();
                }
                catch (Exception ex)
                {
                }
            }
        }


    }

    private void _characteristic_ValueUpdated(object? sender, CharacteristicUpdatedEventArgs e)
    {
        byte[] data = null;
        if(bleData.TryGetValue(e.Characteristic.Name,out data))
        {
            bleData[e.Characteristic.Name] = data.Concat(e.Characteristic.Value).ToArray();
        }
        else
        {
            bleData.Add(e.Characteristic.Name,e.Characteristic.Value);
        }
        MainThread.BeginInvokeOnMainThread(() =>
        {
            var d = string.Join(',', bleData[e.Characteristic.Name]);
            this.logs.Text += $"Data received: {e.Characteristic.Name}=" + d + "\r\n";
            //this.HeartBeatEntry.Text = this.HeartBeatEntry.Text + e.Characteristic.StringValue;
        });


    }
    private void Adapter_DeviceDiscovered(object? sender, DeviceEventArgs e)
    {
        if (e.Device != null && e.Device.Name != null && !devices.Any(p => p.Id == e.Device.Id))
            devices.Add(e.Device);
    }

    private void Current_StateChanged(object? sender, Plugin.BLE.Abstractions.EventArgs.BluetoothStateChangedArgs e)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            this.logs.Text += $"Bluetooth status: {e.NewState} " + "\r\n";
        });
        if (e.NewState == BluetoothState.Off)
        {
            this.devices.Clear();
        }
        else if (e.NewState == BluetoothState.On)
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
}