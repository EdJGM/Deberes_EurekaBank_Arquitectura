using Eurekabank_Maui.ViewModels;
using Eurekabank_Maui.Services;

namespace Eurekabank_Maui.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly LoginViewModel _viewModel;

        public LoginPage()
        {
            InitializeComponent();
            
            _viewModel = new LoginViewModel();
            _viewModel.LoginExitoso += OnLoginExitoso;
            
            BindingContext = _viewModel;
        }

        private async void OnLoginExitoso(object sender, IEurekabankService service)
        {
            // Navegar a la página principal pasando el servicio
            await Navigation.PushAsync(new MainPage(service));
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _viewModel.LoginExitoso -= OnLoginExitoso;
        }
    }
}
