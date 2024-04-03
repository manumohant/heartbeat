using Android.Bluetooth;
using Android.Content;
using Android.Widget;
using Java.IO;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDPredictor.Platforms.Android
{
    public class BluetoothReceiver : BroadcastReceiver
    {
        public BluetoothReceiver()
        {

            MainPage.MainPageDeviceConnectRequest += MainPage_MainPageDeviceConnectRequest;
        }
        private async void MainPage_MainPageDeviceConnectRequest(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (MainActivity.discoveredDevices.Any(p => p.Name.Equals(e.PropertyName)))
            {
                var device = MainActivity.discoveredDevices.FirstOrDefault(p => p.Name == e.PropertyName);
                await StartReceivingData(device);
            }
        }
        private async Task StartReceivingData(BluetoothDevice device)
        {
            if (device.Name != null )
            {
                var bonded = device.BondState;
                if(bonded != Bond.Bonded)
                {
                    AppShell.BTLog($"{device.Name} is not paired, trying to pair");
                    var isbonding = device.CreateBond();
                    if (isbonding)
                    {
                        AppShell.BTLog($"{device.Name} pairing started. Try connecting again after pairing");
                    }
                    else
                    {
                        AppShell.BTLog($"unable to pair {device.Name}");
                    }
                    return;
                }
                
                try
                {
                    BluetoothAdapter bluetoothAdapter = BluetoothAdapter.DefaultAdapter;
                    BluetoothDevice btd = bluetoothAdapter.GetRemoteDevice(device.Address);
                    UUID uuid = UUID.RandomUUID();
                    var socket = btd.CreateRfcommSocketToServiceRecord(uuid);

                    var inputStream = socket.InputStream;
                    byte[] buffer = new byte[1024];
                    int bytes;

                    while (true)
                    {
                        bytes = await inputStream.ReadAsync(buffer, 0, buffer.Length);
                        string data = Encoding.ASCII.GetString(buffer, 0, bytes);
                        if (!string.IsNullOrWhiteSpace(data))
                        {
                            AppShell.BTDataReceived(data);
                            break;
                        }
                        
                    }

                }
                catch (Exception e)
                {

                    AppShell.BTError($"{device.Name} says {e.Message}");
                }
            }
        }
        public async override void OnReceive(Context context, Intent intent)
        {
            var action = intent.Action;

            if (action == BluetoothDevice.ActionFound)
            {
                var device = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);                
                
                if (!string.IsNullOrWhiteSpace(device.Name))
                {                    
                    AppShell.BTDevices(device?.Name);
                    MainActivity.discoveredDevices.Add(device);
                }
                    
            }
            else if(action == BluetoothDevice.ActionAclConnected)
            {
                var device = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);
                Toast.MakeText(context, $"Device {device.Name} connected", ToastLength.Long).Show();
                AppShell.BTLog($"{device.Name} connected");
                await StartReceivingData(device);
            }
            else if (action == BluetoothDevice.ActionAclDisconnected)
            {
                var device = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);
                AppShell.BTLog($"{device.Name} disconnected");
                Toast.MakeText(context, $"Device {device.Name} disconnected", ToastLength.Long).Show();

            }
        }
    }
}
