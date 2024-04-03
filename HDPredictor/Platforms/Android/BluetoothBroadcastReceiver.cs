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

            //MainPage.MainPageDeviceConnectRequest += MainPage_MainPageDeviceConnectRequest;
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
                    
                    var isbonding = device.CreateBond();
                    
                    return;
                }
                
                try
                {
                    BluetoothAdapter bluetoothAdapter = BluetoothAdapter.DefaultAdapter;
                    BluetoothDevice btd = bluetoothAdapter.GetRemoteDevice(device.Address); 
                    var x = btd.FetchUuidsWithSdp();
                    UUID uuid = UUID.FromString("00001101-0000-1000-8000-00805F9B34FB");//serial
                    //var existingId = btd.GetUuids()?.LastOrDefault();
                    //if(existingId!=null) uuid = existingId.Uuid;
                    var socket = btd.CreateRfcommSocketToServiceRecord(uuid);

                    //await socket.ConnectAsync();
                    if (true)
                    {
                        var inputStream = socket.InputStream;
                        byte[] buffer = new byte[1024];
                        int bytes;

                        while (true)
                        {
                            bytes = await inputStream.ReadAsync(buffer, 0, buffer.Length);
                            string data = Encoding.ASCII.GetString(buffer, 0, bytes);
                            if (!string.IsNullOrWhiteSpace(data))
                            { 
                                break;
                            }

                        }
                    }
                        


                    

                }
                catch (Exception e)
                {
                     
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
                    MainActivity.discoveredDevices.Add(device);
                }
                    
            }
            else if(action == BluetoothDevice.ActionAclConnected)
            {
                var device = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);
                Toast.MakeText(context, $"Device {device.Name} connected", ToastLength.Long).Show(); 
                //await StartReceivingData(device);
            }
            else if (action == BluetoothDevice.ActionAclDisconnected)
            {
                var device = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice); 
                Toast.MakeText(context, $"Device {device.Name} disconnected", ToastLength.Long).Show();

            }
        }
    }
}
