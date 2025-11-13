namespace Eurekabank_Maui.Models
{
    public class Movimiento
    {
        public string Cuenta { get; set; }
        public int NroMov { get; set; }
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; }
        public string Accion { get; set; }
        public double Importe { get; set; }

        public string FechaFormateada => Fecha.ToString("dd/MM/yyyy HH:mm");
        public string ImporteFormateado => $"S/. {Importe:N2}";
        public string AccionColor => Accion == "INGRESO" ? "#4CAF50" : "#F44336";
    }
}
