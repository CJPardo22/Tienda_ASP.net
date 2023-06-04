using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Ventas1.Models
{
    public partial class Producto
    {
        public Producto()
        {
            Facturas = new HashSet<Factura>();
        }

        public int Codigo { get; set; }

        [Display(Name = "Producto")]
        public string Nombre { get; set; } = null!;
        public double Precio { get; set; }
        public int Cantidad { get; set; }

        public string NombreProducto
        {
            get { return $"{Nombre} - {Codigo}"; }
        }

        public virtual ICollection<Factura> Facturas { get; set; }
    }
}
