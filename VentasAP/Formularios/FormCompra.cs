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
    public partial class FormCompra : Form
    {
        private apventasEntities db = new apventasEntities();
        private Helpers h = new Helpers();
        int idProducto = 0;
        public FormCompra()
        {
            InitializeComponent();
            cargarLocales();
        }
        //cargar locales
        private void cargarLocales()
        {
            var listaLocales = (from l in db.Local select new { l.id_local, l.nombre }).ToList();

            //cargar la lista al combobox
            cbLocal.DataSource = listaLocales;
            cbLocal.DisplayMember = "nombre";
            cbLocal.ValueMember = "id_local";

            cbLocal.SelectedIndex = -1;
        }
        private void verificarCodigo(string codigo)
        {
            //verifica si el código existe 
            var producto = db.Producto.FirstOrDefault(p => p.codigo == codigo);
            if (producto != null)
            {
                //asigna el nombre del producto al textbox
                txtNombre.Text = producto.nombre;
                //asigna el idProducto a la variable
                idProducto = producto.id_producto;
            }
            else
            {
                MessageBox.Show("El código ingresado no existe");
                limpiarProducto();

            }
        }
        private void limpiarProducto()
        {
            txtNombre.Text = "";
            idProducto = 0;
            txtCantidad.Text = "";
            txtCompra.Text = "";
            txtDescripcion.Text = "";
            txtCodigo.Text = "";
        }

        private void txtCodigo_Leave(object sender, EventArgs e)
        {
            if(txtCodigo.Text.Trim() != "")
            {
                verificarCodigo(txtCodigo.Text.Trim());
            }
        }

        private void txtCodigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            //pregunta si la tecla presionada es Enter
            if(e.KeyChar == (char)Keys.Enter)
            {
                if (txtCodigo.Text.Trim() != "")
                {
                    verificarCodigo(txtCodigo.Text.Trim());
                }
            }
        }

        private void txtFactura_KeyPress(object sender, KeyPressEventArgs e)
        {
            h.soloNumeros(e);
        }

        private void txtCompra_KeyPress(object sender, KeyPressEventArgs e)
        {
            h.soloNumeros(e);
        }

        private void txtCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            h.soloNumeros(e);
        }
    }

}
