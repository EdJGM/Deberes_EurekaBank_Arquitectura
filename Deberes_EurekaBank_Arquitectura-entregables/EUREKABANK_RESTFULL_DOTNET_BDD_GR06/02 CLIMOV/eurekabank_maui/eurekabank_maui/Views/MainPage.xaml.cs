using Eurekabank_Maui.Services;
using Eurekabank_Maui.ViewModels;

namespace Eurekabank_Maui.Views
{
    public partial class MainPage : ContentPage
    {
        private readonly MainViewModel _viewModel;

        public MainPage(IEurekabankService service)
        {
            InitializeComponent();
            
            _viewModel = new MainViewModel(service);
            _viewModel.CerrarSesionSolicitado += OnCerrarSesion;
            
            BindingContext = _viewModel;
        }

        private async void OnCerrarSesion(object sender, EventArgs e)
        {
            // Navegar de vuelta a login
            await Navigation.PopToRootAsync();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _viewModel.CerrarSesionSolicitado -= OnCerrarSesion;
        }
    }
}
