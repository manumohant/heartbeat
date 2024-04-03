namespace HDPredictor;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
		if(!string.IsNullOrWhiteSpace(userName.Text) && !string.IsNullOrWhiteSpace(password.Text) && userName.Text.Equals("Doctor") && password.Text.Equals("Doctor"))
		{
			Navigation.PushAsync(new MainPage(),true); 
		}
		else
		{
			this.error.IsVisible = true;
		}
    }
}