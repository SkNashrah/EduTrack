namespace ExamPortal
{
    public partial class App : Application
    {
        public App()
        {
            try
            {
                InitializeComponent();
                MainPage = new NavigationPage(new LoginPage())
                {
                    BarBackground = (Color)Application.Current.Resources["RrPrimary"]
                };

                ExamPortal.ViewModels.MockData.InitializeData();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}