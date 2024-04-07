using HDPredictor.Services;

namespace HDPredictor;

public partial class LoginPage : ContentPage
{
    public DatabaseService databaseService;
    private bool _isLoggingIn;
    public bool IsLoggingIn
    {
        get { return _isLoggingIn; }
        set { _isLoggingIn = value; OnPropertyChanged(); }
    }
    public LoginPage()
    {
        databaseService = new DatabaseService();
        BindingContext = this;
        InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
        this.IsLoggingIn = true;

        if (!string.IsNullOrWhiteSpace(userName.Text) && !string.IsNullOrWhiteSpace(password.Text) )
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
        IsLoggingIn = false;
    }
}