using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Eurekabank_Soap_Dotnet.ec.edu.monster.modelo
{
    [DataContract(Name="movimiento")]
    public class Movimiento
    {
        [DataMember(Order = 1)]
        public string Cuenta { get; set; }

        [DataMember(Order = 2)]
        public int NroMov { get; set; }

        [DataMember(Order = 3)]
        public DateTime Fecha { get; set; }

        [DataMember(Order = 4)]
        public string Tipo { get; set; }

        [DataMember(Order = 5)]
        public string Accion { get; set; }

        [DataMember(Order = 6)]
        public double Importe { get; set; }

        public Movimiento() { }

        public Movimiento(string cuenta, int nromov, DateTime fecha, string tipo, string accion, double importe)
        {
            Cuenta = cuenta;
            NroMov = nromov;
            Fecha = fecha;
            Tipo = tipo;
            Accion = accion;
            Importe = importe;
        }
    }
}