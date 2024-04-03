using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using HDPredictor.Platforms.Android;

namespace HDPredictor
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        public static List<BluetoothDevice> discoveredDevices = new List<BluetoothDevice>();
        protected override async void OnStart()
        {
            base.OnStart();
        }
        protected async override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            MainPage.MainPageLoaded += MainPage_MainPageLoaded;
        }

        

        private async void MainPage_MainPageLoaded(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            discoveredDevices = new List<BluetoothDevice>();
            var nbDevices = await Permissions.CheckStatusAsync<Permissions.Bluetooth>();
            if (nbDevices != PermissionStatus.Granted)
            {
                nbDevices = await Permissions.RequestAsync<Permissions.Bluetooth>();
                if (nbDevices != PermissionStatus.Granted)
                {
                    SemanticScreenReader.Announce("You can not use this application without a bluetooth connection");
                }
            }

            var bluetoothAdapter = BluetoothAdapter.DefaultAdapter;

            if (bluetoothAdapter.IsDiscovering)
            {
                bluetoothAdapter.CancelDiscovery();
            }
            bluetoothAdapter.StartDiscovery();
            var alreadyConnectedDevices = bluetoothAdapter.BondedDevices;
            if (alreadyConnectedDevices != null)
            {
                foreach (var dev in alreadyConnectedDevices)
                {
                    discoveredDevices.Add(dev);
                    AppShell.BTDevices(dev.Name);
                }
                    
            }
            var receiver = new BluetoothReceiver();


            RegisterReceiver(receiver, new IntentFilter(BluetoothDevice.ActionFound));
            RegisterReceiver(receiver, new IntentFilter(BluetoothDevice.ActionAclConnected));
            RegisterReceiver(receiver, new IntentFilter(BluetoothDevice.ActionAclDisconnected));
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
