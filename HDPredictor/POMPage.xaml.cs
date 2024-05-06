using HDPredictor.Models;
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
    private HDModel _model;
    private List<int> readings = new List<int>();
    public POMPage(HDModel model)
	{
        _model = model;
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
        if (this.readings.Count == 0)
        {
            await DisplayAlert("Error", "No sufficient pulse oximeter reading ", "Close");
            return;
        }
        _model.ThalACH = this.readings.Sum()/this.readings.Count;
		await Navigation.PushAsync(new ECGPage(_model), true);
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

        var suitableCharacteristsFound = false;
        foreach (var service in services)
        {
            //if (service.Id != Guid.Parse("4fafc201-1fb5-459e-8fcc-c5c9c331914b")) continue;
            

            var _characteristics = await service.GetCharacteristicsAsync();
            foreach (var character in _characteristics)
            {
                //if (character.Id != Guid.Parse("beb5483e-36e1-4688-b7f5-ea07361b26a8")) continue;
                try
                {
                    character.ValueUpdated += _characteristic_ValueUpdated; ;

                    await character.StartUpdatesAsync();
                    suitableCharacteristsFound = true;
                }
                catch (Exception ex)
                {
                }
            }
        }

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            this.logs.Text = "Waiting for data...";
            if(!suitableCharacteristsFound)
            {
                this.logs.Text += "Device ID is not suitable, please select the configured device...";
                await DisplayAlert("Error", "Device ID is not suitable, please select the configured device... ", "Close");
            }
            else
            {
                await DisplayAlert("Success", "Successfully connected. Please wait while we collect pulse oximeter reading ", "Close");
            }
        });
    }

    private void _characteristic_ValueUpdated(object? sender, CharacteristicUpdatedEventArgs e)
    { 
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            var val = Plugin.BLE.DataAdapter.Current.Adaptor.ParseUtf(e.Characteristic.Value);
            readings.Add(val);
            this.logs.Text += $"Data received: {e.Characteristic.Name},stval={e.Characteristic.StringValue},PulseOximeterReading=" + val + "\r\n";
            
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