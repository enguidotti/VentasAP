using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VentasAP.Models
{
    public class Factura
    {
        //pasar los datos que quiere mostrar en el reporte
        public int NumeroFactura { get; set; }
        public string Local { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string Usuario { get; set; }
        public string Codigo { get; set; }
        public string Producto { get; set; }
        public int Cantidad { get; set; }
        public string Precio { get; set; }
        public string TotalProducto { get; set; }
        //creando constructor de la clase para acceder a sus atributos
        public Factura() { }
    }
}
