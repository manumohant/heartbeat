using HDPredictor.Services;

namespace HDPredictor;

public partial class LoginPage : ContentPage
{
    public DatabaseService databaseService;
    public LoginPage()
    {
        databaseService = new DatabaseService();
        InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		if(!string.IsNullOrWhiteSpace(userName.Text) && !string.IsNullOrWhiteSpace(password.Text) )
		{
            var logedId = await databaseService.LoginAsync(userName.Text, password.Text);
            if(logedId)
                Navigation.PushAsync(new MainPage(),true); 
            else
                this.error.IsVisible = true;
        }
		else
		{
			this.error.IsVisible = true;
		}
    }
}