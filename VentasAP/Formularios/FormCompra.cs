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
            if (txtCodigo.Text.Trim() != "")
            {
                verificarCodigo(txtCodigo.Text.Trim());
            }
        }

        private void txtCodigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            //pregunta si la tecla presionada es Enter
            if (e.KeyChar == (char)Keys.Enter)
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

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (idProducto > 0)
            {
                string error = validarProducto();
                if(error != "")
                {
                    MessageBox.Show(error,"Validación",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }
                else
                {
                    bool existeCodigo = false;//si no existe código en la grilla se mantiene en falso
                    //calcula el valor total(cantidad*precio_compra)
                    int total = int.Parse(txtCantidad.Text) * int.Parse(txtCompra.Text);
                    //verfica si la grilla trae datos
                    if (dgvDetalle.Rows.Count > 0)
                    {
                        //**verifica si el producto ya esta ingresado**
                        //se recorren las filas de la grilla
                        foreach (DataGridViewRow f in dgvDetalle.Rows)
                        {
                            //verifica si el código existe en la grilla
                            if (f.Cells[0].Value.ToString() == txtCodigo.Text)
                            {
                                existeCodigo = true;//existeCodigo cambia a verdadero, para no agregar un nuevo producto
                                //ya que encuentra código, le preguntamos si desea actualziar los datos
                                var resp = MessageBox.Show("El código ya esta ingresado,¿desea modificarlo?", "Modificar",
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                                //si la respuesta es sí, se actualizan los datos
                                if (resp == DialogResult.Yes)
                                {
                                    //captura el valor total 
                                    int totalAnt = int.Parse(f.Cells[4].Value.ToString());
                                    f.Cells[2].Value = txtCantidad.Text;
                                    f.Cells[3].Value = txtCompra.Text;
                                    f.Cells[4].Value = total;
                                    f.Cells[5].Value = txtDescripcion.Text.Trim();
                                    //al valor total se le resta el valor a modifcar y se le suma el nuevo
                                    txtTotal.Text = (int.Parse(txtTotal.Text) + total - totalAnt).ToString();
                                }
                                break;//se utiliza para detener el ciclo
                            }
                        }
                    }
                    //si código es falso crear agrega un nuevo producto
                    if (!existeCodigo)//existeCodigo == false <==> !existeCodigo
                    {
                        //agregar un producto a la grilla
                        DataGridViewRow fila = new DataGridViewRow();
                        fila.CreateCells(dgvDetalle);//se crea la fila
                        //se agregan los elementos de la fila 
                        fila.Cells[0].Value = txtCodigo.Text;
                        fila.Cells[1].Value = txtNombre.Text;
                        fila.Cells[2].Value = txtCantidad.Text;
                        fila.Cells[3].Value = txtCompra.Text;
                        fila.Cells[4].Value = total;
                        fila.Cells[5].Value = txtDescripcion.Text.Trim();
                        fila.Cells[6].Value = idProducto;
                        //se añade la fila a grilla
                        dgvDetalle.Rows.Add(fila);
                        //muestra el total a pagar de nuestra orden ingresada
                        txtTotal.Text = (int.Parse(txtTotal.Text) + total).ToString();
                    }
                    limpiarProducto();
                }
            }
        }

        private string validarProducto()
        {
            string error = "";
            if (string.IsNullOrEmpty(txtCodigo.Text.Trim()))
            {
                error = "Debe ingresar un producto \n";
            }
            if (string.IsNullOrEmpty(txtCantidad.Text))
            {
                error += "Debe ingresar cantidad del producto \n";
            }
            if (string.IsNullOrEmpty(txtCompra.Text))
            {
                error += "Debe ingresar precio compra del producto \n";
            }
            return error;
        }

        private void productoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();//se oculta la ventana actual
            FormProducto producto = new FormProducto();//se instancia la clase(formulario)
            producto.Show();//se muestra la ventana

        }
    }

}
