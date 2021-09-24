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
    public partial class FormProducto : Form
    {
        private apventasEntities db = new apventasEntities();
        int idProducto = 0;
        public FormProducto()
        {
            InitializeComponent();
            cargarMarcas();
            cargarCategorias();
        }
        //métodos para marcas a combobox
        private void cargarMarcas()
        {
            //consulta que trae todas las marcas
            var listaMarcas = (from m in db.Marca
                               orderby m.nombre
                               select new
                               {
                                   id = m.id_marca,
                                   Nombre = m.nombre
                               }).ToList();
            //se asigna la lista de marcas al combobox
            cbMarcas.DataSource = listaMarcas;
            //se asigna que atributo será el valor del cb
            cbMarcas.ValueMember = "id";
            //se asigna el atributo visible por el usuario al cb
            cbMarcas.DisplayMember = "Nombre";
            //no queda seleccionado ningún elemento dentro del cb
            cbMarcas.SelectedIndex = -1;
        }

        private void cargarCategorias()
        {
            //select * from categoria 
            var listaCategorias = db.Categoria.OrderBy(c => c.nombre).ToList();

            cbCategorias.DataSource = listaCategorias;
            cbCategorias.ValueMember = "id_categoria";
            cbCategorias.DisplayMember = "nombre";

            cbCategorias.SelectedIndex = -1;
        }

        private string Validar()
        {
            string mensaje = "";
            //string.IsNullOrEmpty verifica si un string es nulo o vacio
            if (string.IsNullOrEmpty(cbMarcas.Text.Trim()))
                mensaje = "Debe seleccionar una Marca \n";
            if (string.IsNullOrEmpty(cbCategorias.Text.Trim()))
                mensaje += "Debe seleccionar una Categoría \n";
            if (string.IsNullOrEmpty(txtCodigo.Text.Trim()))
                mensaje += "Debe ingresar un Código \n";
            if (string.IsNullOrEmpty(txtNombre.Text.Trim()))
                mensaje += "Debe ingresar un Nombre \n";
            if (string.IsNullOrEmpty(txtCompra.Text.Trim()))
                mensaje += "Debe ingresar un Precio de compra \n";
            if (string.IsNullOrEmpty(txtVenta.Text.Trim()))
                mensaje += "Debe ingresar un Precio de Venta \n";
            return mensaje;
        }
        private void Guardar()
        {
            Producto p = new Producto();
            p.codigo = txtCodigo.Text.Trim();
            p.nombre = txtNombre.Text.Trim();
            p.precio_compra = int.Parse(txtCompra.Text);
            p.precio_venta = int.Parse(txtVenta.Text);
            p.descripcion = txtDescripcion.Text.Trim();
            p.id_marca = int.Parse(cbMarcas.SelectedValue.ToString());
            p.id_categoria = int.Parse(cbCategorias.SelectedValue.ToString());

            db.Producto.Add(p);
            db.SaveChanges();
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string error = Validar();
            if(error != "")
            {
                MessageBox.Show(error, "Falta datos");
            } 
            else
            {
                if(idProducto == 0)
                {
                    //guardar
                    Guardar();
                } else
                {
                    //modificar
                }
            }
        }
    }
}
