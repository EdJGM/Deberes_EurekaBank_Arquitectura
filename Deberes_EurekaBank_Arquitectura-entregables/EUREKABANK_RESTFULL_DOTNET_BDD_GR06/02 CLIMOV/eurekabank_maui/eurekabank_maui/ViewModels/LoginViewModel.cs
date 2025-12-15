using Eurekabank_Maui.Models;
using Eurekabank_Maui.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Eurekabank_Maui.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _username;
        private string _password;
        private ServidorConfig _servidorSeleccionado;
        private string _mensajeError;
        private bool _mostrarError;
        private string _iconoColor = "#512BD4";

        public LoginViewModel()
        {
            Title = "Eurekabank - Login";
            Servidores = new ObservableCollection<ServidorConfig>(ServidorConfig.ObtenerServidores());
            LoginCommand = new Command(async () => await ExecuteLoginAsync(), () => CanLogin());
            VerificarConexionCommand = new Command(async () => await VerificarConexionAsync());
        }

        public ObservableCollection<ServidorConfig> Servidores { get; }

        public string Username
        {
            get => _username;
            set
            {
                if (SetProperty(ref _username, value))
                {
                    ((Command)LoginCommand).ChangeCanExecute();
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (SetProperty(ref _password, value))
                {
                    ((Command)LoginCommand).ChangeCanExecute();
                }
            }
        }

        public ServidorConfig ServidorSeleccionado
        {
            get => _servidorSeleccionado;
            set
            {
                if (SetProperty(ref _servidorSeleccionado, value))
                {
                    ((Command)LoginCommand).ChangeCanExecute();
                    MostrarError = false;
                }
            }
        }

        public string MensajeError
        {
            get => _mensajeError;
            set => SetProperty(ref _mensajeError, value);
        }

        public bool MostrarError
        {
            get => _mostrarError;
            set => SetProperty(ref _mostrarError, value);
        }

        public string IconoColor
        {
            get => _iconoColor;
            set => SetProperty(ref _iconoColor, value);
        }

        public ICommand LoginCommand { get; }
        public ICommand VerificarConexionCommand { get; }

        public event EventHandler<IEurekabankService> LoginExitoso;

        private bool CanLogin()
        {
            return !string.IsNullOrWhiteSpace(Username) &&
                   !string.IsNullOrWhiteSpace(Password) &&
                   ServidorSeleccionado != null &&
                   !IsBusy;
        }

        private async Task ExecuteLoginAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            MostrarError = false;

            try
            {
                var httpClient = MauiProgram.Services.GetService<HttpClient>();
                var service = EurekabankServiceFactory.Create(ServidorSeleccionado, httpClient);

                // Primero verificar si el servidor está disponible
                var serverOk = await service.HealthCheckAsync();
                if (!serverOk)
                {
                    MensajeError = $"No se puede conectar al servidor {ServidorSeleccionado.Nombre}. Verifique que esté en ejecución.";
                    MostrarError = true;
                    return;
                }

                // Intentar login
                var loginExitoso = await service.LoginAsync(Username, Password);

                if (loginExitoso)
                {
                    // Notificar login exitoso y pasar el servicio configurado
                    LoginExitoso?.Invoke(this, service);
                }
                else
                {
                    MensajeError = "Usuario o contraseña incorrectos";
                    MostrarError = true;
                }
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al conectar: {ex.Message}";
                MostrarError = true;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task VerificarConexionAsync()
        {
            if (ServidorSeleccionado == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Advertencia",
                    "Por favor seleccione un servidor",
                    "OK");
                return;
            }

            IsBusy = true;
            MostrarError = false;

            try
            {
                var httpClient = MauiProgram.Services.GetService<HttpClient>();
                var service = EurekabankServiceFactory.Create(ServidorSeleccionado, httpClient);

                // Timeout de 5 segundos con cancelación correcta
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

                try
                {
                    var serverOk = await service.HealthCheckAsync();
                    if (serverOk)
                    {
                        await Application.Current.MainPage.DisplayAlert(
                            "Conexión Exitosa",
                            $"El servidor {ServidorSeleccionado.Nombre} está disponible y responde correctamente",
                            "OK");
                    }
                    else
                    {
                        MensajeError = $"El servidor {ServidorSeleccionado.Nombre} no está disponible o no responde";
                        MostrarError = true;
                    }
                }
                catch (OperationCanceledException)
                {
                    MensajeError = $"Timeout: El servidor {ServidorSeleccionado.Nombre} no respondió en 5 segundos";
                    MostrarError = true;
                }
            }
            catch (Exception ex)
            {
                MensajeError = $"Error de conexión: {ex.Message}";
                MostrarError = true;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
