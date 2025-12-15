using Eurekabank_Maui.Models;
using Eurekabank_Maui.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Eurekabank_Maui.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IEurekabankService _service;
        private string _cuenta;
        private double _importe;
        private string _cuentaDestino;
        private string _mensajeEstado;
        private bool _mostrarMensaje;
        private string _accionColor = "#512BD4";

        public MainViewModel(IEurekabankService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            
            var serverInfo = _service.GetServidorInfo();
            Title = $"Eurekabank - {serverInfo.Nombre}";
            
            Movimientos = new ObservableCollection<Movimiento>();

            // Inicializar comandos
            ConsultarCommand = new Command(async () => await ConsultarMovimientosAsync(), () => !string.IsNullOrWhiteSpace(Cuenta));
            DepositoCommand = new Command(async () => await RealizarDepositoAsync(), () => CanExecuteTransaccion());
            RetiroCommand = new Command(async () => await RealizarRetiroAsync(), () => CanExecuteTransaccion());
            TransferenciaCommand = new Command(async () => await RealizarTransferenciaAsync(), () => CanExecuteTransferencia());
            CerrarSesionCommand = new Command(async () => await CerrarSesionAsync());
        }

        public ObservableCollection<Movimiento> Movimientos { get; }

        public ServidorConfig ServidorInfo => _service.GetServidorInfo();

        public string Cuenta
        {
            get => _cuenta;
            set
            {
                if (SetProperty(ref _cuenta, value))
                {
                    UpdateCommandStates();
                }
            }
        }

        public double Importe
        {
            get => _importe;
            set
            {
                if (SetProperty(ref _importe, value))
                {
                    UpdateCommandStates();
                }
            }
        }

        public string CuentaDestino
        {
            get => _cuentaDestino;
            set
            {
                if (SetProperty(ref _cuentaDestino, value))
                {
                    ((Command)TransferenciaCommand).ChangeCanExecute();
                }
            }
        }

        public string MensajeEstado
        {
            get => _mensajeEstado;
            set => SetProperty(ref _mensajeEstado, value);
        }

        public bool MostrarMensaje
        {
            get => _mostrarMensaje;
            set => SetProperty(ref _mostrarMensaje, value);
        }

        public string AccionColor
        {
            get => _accionColor;
            set => SetProperty(ref _accionColor, value);
        }

        public ICommand ConsultarCommand { get; }
        public ICommand DepositoCommand { get; }
        public ICommand RetiroCommand { get; }
        public ICommand TransferenciaCommand { get; }
        public ICommand CerrarSesionCommand { get; }

        public event EventHandler CerrarSesionSolicitado;

        private bool CanExecuteTransaccion()
        {
            return !string.IsNullOrWhiteSpace(Cuenta) && Importe > 0 && !IsBusy;
        }

        private bool CanExecuteTransferencia()
        {
            return CanExecuteTransaccion() && !string.IsNullOrWhiteSpace(CuentaDestino);
        }

        private void UpdateCommandStates()
        {
            ((Command)ConsultarCommand).ChangeCanExecute();
            ((Command)DepositoCommand).ChangeCanExecute();
            ((Command)RetiroCommand).ChangeCanExecute();
            ((Command)TransferenciaCommand).ChangeCanExecute();
        }

        private async Task ConsultarMovimientosAsync()
        {
            if (IsBusy || string.IsNullOrWhiteSpace(Cuenta))
                return;

            IsBusy = true;
            MostrarMensaje = false;

            try
            {
                var movimientos = await _service.ObtenerMovimientosAsync(Cuenta);
                
                Movimientos.Clear();
                foreach (var mov in movimientos)
                {
                    Movimientos.Add(mov);
                }

                if (Movimientos.Count == 0)
                {
                    MensajeEstado = "No se encontraron movimientos para esta cuenta";
                    MostrarMensaje = true;
                }
                else
                {
                    MensajeEstado = $"Se encontraron {Movimientos.Count} movimientos";
                    MostrarMensaje = true;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    $"Error al consultar movimientos: {ex.Message}",
                    "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task RealizarDepositoAsync()
        {
            if (!CanExecuteTransaccion())
                return;

            var confirmado = await Application.Current.MainPage.DisplayAlert(
                "Confirmar Depósito",
                $"¿Desea depositar S/. {Importe:N2} en la cuenta {Cuenta}?",
                "Sí",
                "No");

            if (!confirmado)
                return;

            IsBusy = true;
            MostrarMensaje = false;

            try
            {
                var exitoso = await _service.RegistrarDepositoAsync(Cuenta, Importe);

                if (exitoso)
                {
                    MensajeEstado = $"Depósito de S/. {Importe:N2} realizado exitosamente";
                    MostrarMensaje = true;
                    Importe = 0;
                    
                    // Refrescar movimientos
                    await ConsultarMovimientosAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        "No se pudo realizar el depósito",
                        "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    $"Error al realizar depósito: {ex.Message}",
                    "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task RealizarRetiroAsync()
        {
            if (!CanExecuteTransaccion())
                return;

            var confirmado = await Application.Current.MainPage.DisplayAlert(
                "Confirmar Retiro",
                $"¿Desea retirar S/. {Importe:N2} de la cuenta {Cuenta}?",
                "Sí",
                "No");

            if (!confirmado)
                return;

            IsBusy = true;
            MostrarMensaje = false;

            try
            {
                var exitoso = await _service.RegistrarRetiroAsync(Cuenta, Importe);

                if (exitoso)
                {
                    MensajeEstado = $"Retiro de S/. {Importe:N2} realizado exitosamente";
                    MostrarMensaje = true;
                    Importe = 0;
                    
                    // Refrescar movimientos
                    await ConsultarMovimientosAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        "No se pudo realizar el retiro",
                        "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    $"Error al realizar retiro: {ex.Message}",
                    "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task RealizarTransferenciaAsync()
        {
            if (!CanExecuteTransferencia())
                return;

            var confirmado = await Application.Current.MainPage.DisplayAlert(
                "Confirmar Transferencia",
                $"¿Desea transferir S/. {Importe:N2} de la cuenta {Cuenta} a la cuenta {CuentaDestino}?",
                "Sí",
                "No");

            if (!confirmado)
                return;

            IsBusy = true;
            MostrarMensaje = false;

            try
            {
                var exitoso = await _service.RegistrarTransferenciaAsync(Cuenta, CuentaDestino, Importe);

                if (exitoso)
                {
                    MensajeEstado = $"Transferencia de S/. {Importe:N2} realizada exitosamente";
                    MostrarMensaje = true;
                    Importe = 0;
                    CuentaDestino = string.Empty;
                    
                    // Refrescar movimientos
                    await ConsultarMovimientosAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        "No se pudo realizar la transferencia",
                        "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    $"Error al realizar transferencia: {ex.Message}",
                    "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task CerrarSesionAsync()
        {
            var confirmado = await Application.Current.MainPage.DisplayAlert(
                "Cerrar Sesión",
                "¿Está seguro que desea cerrar sesión?",
                "Sí",
                "No");

            if (confirmado)
            {
                CerrarSesionSolicitado?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
