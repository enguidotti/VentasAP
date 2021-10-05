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
                txtCompra.Text = producto.precio_compra.ToString();
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
                if (error != "")
                {
                    MessageBox.Show(error, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string error = "";
            if (string.IsNullOrEmpty(cbLocal.Text))
            {
                error = "Debe seleccionar un local \n";
            }
            if (string.IsNullOrEmpty(txtFactura.Text))
            {
                error += "Debe ingresar el número de factura \n";
            }
            if (dgvDetalle.Rows.Count == 0)//verifica si no tiene productos
            {
                error += "Debe ingresar al menos un producto a la orden";
            }
            if (error != "")
            {
                MessageBox.Show(error, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                //se asigna el local seleccionado a un variable entera
                int idLocal = int.Parse(cbLocal.SelectedValue.ToString()); 
                //guardar en la tabla OrdenCompra
                OrdenCompra orden = new OrdenCompra();//instanciar clase para acceder a sus métodos y atributos
                //asignar valores a los atributos
                orden.id_local = idLocal;
                orden.fecha = dtFecha.Value;
                orden.num_factura = int.Parse(txtFactura.Text);
                orden.id_user = 1; //luego esto cambia y se utilizar el usuario logeado en el sistema
                db.OrdenCompra.Add(orden);
                db.SaveChanges();
                //una vez guardado en DB podemos acceder al clave primaria auto generada
                int idOrden = orden.id_orden;

                //guardar en tabla detalle de compra
                DetalleIngreso detalle = new DetalleIngreso();
                //recorre todas las filas de nuestra grilla 
                foreach (DataGridViewRow fila in dgvDetalle.Rows)
                { 
                    //asignamos valores a las variables
                    int cantidad = int.Parse(fila.Cells[2].Value.ToString());
                    int idProducto = int.Parse(fila.Cells[6].Value.ToString());       
                    //invoca al método verificaStock para obtener el idStock del producto según el local
                    int idStock = VerificaStock(idProducto,idLocal,cantidad);
                    detalle.id_stock = idStock;
                    detalle.cantidad = cantidad;
                    detalle.precio_compra = int.Parse(fila.Cells[3].Value.ToString());
                    detalle.descripcion = fila.Cells[5].Value.ToString();
                    detalle.id_orden = idOrden;

                    db.DetalleIngreso.Add(detalle);
                    db.SaveChanges();
                }
                MessageBox.Show("Los registros se han agregado con éxito!");
                Limpiar();
            }
        }

        private int VerificaStock(int idProducto, int idLocal,int cantidad)
        {
            //consulta que verifica la existencia del producto en el local seleccionado
            var query = db.StockLocal.FirstOrDefault(s => s.id_producto == idProducto && s.id_local == idLocal);
            if(query == null)//que el producto en ese local no existe
            {
                StockLocal stock = new StockLocal();
                stock.cantidad = cantidad;
                stock.id_local = idLocal;
                stock.id_producto = idProducto;

                db.StockLocal.Add(stock);
                db.SaveChanges();
                //devuelve el id_stock que se genera al ingresar el registro
                return stock.id_stock;
            } 
            else
            {
                //actualiza el stock existente,añadiendole la cantidad ingresada
                query.cantidad = query.cantidad + cantidad;
                db.SaveChanges();
                //si existe el registro se retorna el id_stock del registro encontrado
                return query.id_stock;
            }
        }
        private void Limpiar()
        {
            cbLocal.SelectedIndex = -1;
            dtFecha.Value = DateTime.Now;
            txtFactura.Text = "";
            txtTotal.Text = "0";
            //limpia la grilla
            dgvDetalle.Rows.Clear();
        }
    }

}
