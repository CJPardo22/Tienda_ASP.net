using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Ventas1.Models
{
    public partial class Factura
    {
        public int IdFactura { get; set; }
        public DateTime Fecha { get; set; }
        public int Cantidad { get; set; }
        public double Total { get; set; }
        public int? Cedula { get; set; }
        public int? Producto { get; set; }


        [Display(Name = "Documento")]
        public virtual Cliente? CedulaNavigation { get; set; }

        [Display(Name = "Code")]
        public virtual Producto? ProductoNavigation { get; set; }

    }
}
