namespace HDPredictor;

public partial class FinalPage : ContentPage
{
	public FinalPage()
	{
		InitializeComponent();
	}
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        animation.IsAnimationPlaying = false;
        await Task.Delay(100);
        animation.IsAnimationPlaying = true;
        await Task.Delay(1000*15); // this is from where we should call the Azure ML endpoint
        this.animation.IsVisible = false;
        this.caption.IsVisible = false;
        this.result.IsVisible = true;
    }
}