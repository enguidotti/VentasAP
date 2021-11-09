using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VentasAP.Models;

namespace VentasAP.Formularios
{
    public partial class FormReporte : Form
    {
        private apventasEntities db = new apventasEntities();
        public FormReporte()
        {
            InitializeComponent();
        }
        private void cargarColores()
        {
            //recorre todas los controles o herramientas que hay en el formulario
            foreach (Control btns in Controls)
            {
                //verifica que los controles sean del tipo Button(que sea un botón)
                if (btns.GetType() == typeof(Button))
                {
                    Button btn = (Button)btns;
                    //COlores es la clase con la lista de colores
                    btn.BackColor = Colores.PrimaryColor;
                    btn.ForeColor = Color.White;
                    btn.FlatAppearance.BorderColor = Colores.SecondaryColor;
                }
                lblTitulo.ForeColor = Colores.SecondaryColor;

            }
        }
        private void FormReporte_Load(object sender, EventArgs e)
        {
            cargarColores();
            this.rvFactura.RefreshReport();
        }

        private void CargarReporte(int numFactura)
        {
            //consulta que determina la orden de compra en base al numero de factura
            var orden = (from o in db.DetalleIngreso
                         where o.OrdenCompra.num_factura == numFactura
                         select new
                         {
                             o.OrdenCompra.num_factura,
                             o.OrdenCompra.fecha,
                             local = o.OrdenCompra.Local.nombre,
                             user = o.OrdenCompra.User.nombres + " " + o.OrdenCompra.User.apellidos,
                             o.StockLocal.Producto.codigo,
                             producto = o.StockLocal.Producto.nombre,
                             o.cantidad,
                             o.precio_compra
                         });
            if (orden.Count() > 0)
            {
                //se asignan los datos a mi clase auxiliar FACTURA para posteriormente ser asignadas al reporte
                List<Factura> factura = new List<Factura>();
                foreach (var item in orden)
                {
                    factura.Add(new Factura
                    {
                        NumeroFactura = item.num_factura,
                        FechaIngreso = item.fecha,
                        Local = item.local,
                        Usuario = item.user,
                        Codigo = item.codigo,
                        Producto = item.producto,
                        Cantidad = item.cantidad,
                        Precio = item.precio_compra.ToString("C"),
                        TotalProducto = (item.cantidad * item.precio_compra).ToString("C")
                    });
                }
                ReportDataSource report = new ReportDataSource("Factura", factura);
                rvFactura.LocalReport.DataSources.Clear();
                rvFactura.LocalReport.DataSources.Add(report);
                rvFactura.RefreshReport();
                rvFactura.Visible = true;
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            int numFactura = int.Parse(txtFactura.Text);
            CargarReporte(numFactura);
        }
    }
}
