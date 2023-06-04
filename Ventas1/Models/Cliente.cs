using System;
using System.Collections.Generic;

namespace Ventas1.Models
{
    public partial class Cliente
    {
        public Cliente()
        {
            Facturas = new HashSet<Factura>();
        }

        public int Cedula { get; set; }
        public string Nombre { get; set; } = null!;
        public string Direccion { get; set; } = null!;
        public string Telefono { get; set; } = null!;

        public virtual ICollection<Factura> Facturas { get; set; }

        //Este codigo es mio 
        public string NombreCedula
        {
            get { return $"{Nombre} - {Cedula}"; }
        }
    }
    
}
