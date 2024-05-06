using HDPredictor.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace HDPredictor;

public partial class FinalPage : ContentPage
{
    private HDModel _model;

    public FinalPage(HDModel model)
	{
        this._model = model;
		InitializeComponent();
	}
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        animation.IsAnimationPlaying = false;
        await Task.Delay(100);
        animation.IsAnimationPlaying = true;
        this.logs.Text += "Calling Model endpoint.\r\n";
        var handler = new HttpClientHandler()
        {
            ClientCertificateOptions = ClientCertificateOption.Manual,
            ServerCertificateCustomValidationCallback =
                        (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
        };
        using (var client = new HttpClient(handler))
        {


            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "gTyA7quRQmZmx6oimndTpJhcp8MqSVw3 ");
            client.BaseAddress = new Uri("https://heart-disease-kuvps.centralindia.inference.ml.azure.com/score");
            this.logs.Text += "https://heart-disease-kuvps.centralindia.inference.ml.azure.com/score\r\n";
            var json = System.Text.Json.JsonSerializer.Serialize(
                new
                {
                    data = _model
                }
            );
            var content = new StringContent(json);
            this.logs.Text += $"Request: {json}";
            await Task.Delay(1000 * 10);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            content.Headers.Add("azureml-model-deployment", "heart-disease-1");
            HttpResponseMessage response = await client.PostAsync("", content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var prediction = System.Text.Json.JsonSerializer.Deserialize<PredictionOutcome>(System.Text.Json.JsonSerializer.Deserialize<string>(result));
                this.resultFalse.IsVisible = prediction.predictions[0] == "true";
                this.resultTrue.IsVisible = !(prediction.predictions[0] == "true");
                await DisplayAlert("Success", "Your result is ready to view", "Close");

                this.logs.Text += $"Response: {result}";
            }
            else
            {
                await DisplayAlert("Error", "An unexpected error occured. Plesae try again", "Close");
            }
        }
        this.animation.IsVisible = false;
        this.caption.IsVisible = false;
    }
}