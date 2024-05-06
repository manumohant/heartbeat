
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using HDPredictor.Services;
using HDPredictor.Models; 

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
            try
            {
                var model = new HDModel
                {
                    Email = this.EmailEntry.Text,
                    Name = this.NameEntry.Text,
                    Age = int.Parse(this.AgeEntry.Text),
                    Gender = this.GenderPicker.SelectedIndex,
                    Chestpain = this.ChestPain.SelectedIndex + 1,
                    RestingBloodpressure = float.Parse(this.RestingBP.Text),
                    SerumCholestrol = float.Parse(this.Cholesterol.Text),
                    FastingBloodSugar = this.BloodSugar.SelectedIndex,
                    Exercise = this.ExIndAngina.SelectedIndex
                };
                await Navigation.PushAsync(new POMPage(model), true);
                
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Please fill all the fields","Close");
            }

        }
    }

}
