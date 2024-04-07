
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using HDPredictor.Services;

namespace HDPredictor
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();

        }
        private async void Submit_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new POMPage(),true);
            //DisplayAlert("Sent", "Data sent for processing. Please wait for the result","Close");
        }
    }

}
