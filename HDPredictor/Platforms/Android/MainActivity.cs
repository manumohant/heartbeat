using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.Content.PM;
using Android.DeviceLock;
using Android.OS;
using Android.Widget;
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
            //MainPage.MainPageLoaded += MainPage_MainPageLoaded;
        }

        

        private async void MainPage_MainPageLoaded(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
