namespace HDPredictor
{
    public partial class App : Application
    {
        public static IBlueToothService blueToothService;
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell(); 
#if ANDROID
		blueToothService = new HDPredictor.Platforms.Android.BlueToothServie();
#endif
        }
    }
}
