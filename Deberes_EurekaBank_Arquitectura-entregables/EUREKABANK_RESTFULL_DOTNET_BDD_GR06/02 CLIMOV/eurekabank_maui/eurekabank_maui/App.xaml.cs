using Eurekabank_Maui.Views;

namespace Eurekabank_Maui
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Configurar la página de inicio como NavigationPage con LoginPage
            MainPage = new NavigationPage(new LoginPage())
            {
                BarBackgroundColor = Color.FromArgb("#512BD4"),
                BarTextColor = Colors.White
            };
        }
    }
}
