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
    public partial class FormMarca : Form
    {
        //instancia la conexión a base de datos y asignamos nombre de objeto db
        private apventasEntities db = new apventasEntities();
        int idMarca = 0;//variable que permitirá guardar, modificar o eliminar datos, dependiendo de su valor
        public FormMarca()
        {
            InitializeComponent();
            //llama al método que carga la lista de marcas en la grilla
            cargarMarcas();
            cargarReporte();
        }
        private void cargarReporte()
        {
            var listaMarcas = (from m in db.Marca
                               select new
                               {
                                   m.id_marca,
                                   m.nombre
                               }).ToList();
            if (listaMarcas.Count() > 0)
            {
                ReportDataSource report = new ReportDataSource("Marca", listaMarcas);
                rvMarca.LocalReport.DataSources.Clear();
                rvMarca.LocalReport.DataSources.Add(report);
                rvMarca.RefreshReport();
            }
        }
        private void cargarColores()
        {
            //recorre todas los controles o herramientas que hay en el formulario
            foreach (Control btns in Controls)
            {
                //verifica que los controles sean del tipo Button(que sea un botón)
                if(btns.GetType() == typeof(Button))
                {
                    Button btn = (Button)btns;
                    //COlores es la clase con la lista de colores
                    btn.BackColor = Colores.PrimaryColor;
                    btn.ForeColor = Color.White;
                    btn.FlatAppearance.BorderColor = Colores.SecondaryColor;
                }
                lblTitulo.ForeColor = Colores.SecondaryColor;
                dgvMarcas.ColumnHeadersDefaultCellStyle.BackColor = Colores.PrimaryColor;
                dgvMarcas.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvMarcas.EnableHeadersVisualStyles = false;

                dgvMarcas.Font = new Font("Times", 16);

            }
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (txtNombre.Text.Trim() != "")
            {
                if (idMarca == 0)//sirve para agregar un nuevo registro
                {
                    if (buscarNombre(txtNombre.Text.Trim()))
                    {
                        //instancia la clase marca
                        Marca marca = new Marca();
                        //asigno al atributo nombre el texto ingresado desde el formulario
                        marca.nombre = txtNombre.Text.Trim();  //.Trim() para todos los lenguaje y quita los espacios en blanco
                                                               //se añade el registro a la base de dato
                        db.Marca.Add(marca);//INSERT INTO Marca(nombre) values('Apple');
                                            //guardar los cambios realizados a la base de datos
                        db.SaveChanges();
                        limpiar();
                        cargarMarcas();
                    }
                    else
                    {
                        MessageBox.Show("La marca ya está registrada","Error",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    }
                }
                else//modifica un registro existente
                {
                    if (buscarNombre(txtNombre.Text.Trim()))
                    {
                        //Find sirve para buscar en una tabla por la primary key
                        var marca = db.Marca.Find(idMarca);
                        //verifica que exista el registro
                        if (marca != null)
                        {
                            //se asigna el nuevo valor
                            marca.nombre = txtNombre.Text;
                            //se guardan los cambios en la base de datos
                            db.SaveChanges();
                            limpiar();
                            cargarMarcas();
                        }
                    }
                    else
                    {
                        MessageBox.Show("La marca ya está registrada", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe ingrese un nombre de marca");
            }
        }
        //método para verifica si el nombre existe en la bd
        private bool buscarNombre(string nombre)
        {
            //SELECT TOP(1) * FROM Marca m WHERE m.nombre = 'Lenovo'
            //FirstOrDefault trae el primer elemento con coincidencia de la base de datos
            var q = db.Marca.FirstOrDefault(m=>m.nombre == nombre);
            //verifica si trae registro esa marca
            if(q != null)
            {
                //retorna false para indicar que existe un valor
                return false;
            }
            //retorna true para luego poder acceder a guardar el dato, si no existe
            return true;
        }
        //método para cargar los datos en la grilla
        private void cargarMarcas()
        {
            //SELECT * FROM Marca; pero en entityframework
            //var listaMarcas = db.Marca.ToList();
            //otra forma de hacerlo
            var listaMarcas = (from m in db.Marca
                               select new
                               {
                                   Id = m.id_marca,
                                   Nombre = m.nombre
                               }).ToList();
            //añadir la lista a la grilla
            dgvMarcas.DataSource = listaMarcas;
            dgvMarcas.Columns[0].Visible = false;//permite ocultar colummnas

            
        }

        private void dgvMarcas_MouseClick(object sender, MouseEventArgs e)
        {
            //asignan los valores de las filas a las variables o textbox
            idMarca = int.Parse(dgvMarcas.CurrentRow.Cells[0].Value.ToString());
            txtNombre.Text = dgvMarcas.CurrentRow.Cells[1].Value.ToString();

            btnEliminar.Enabled = true;
        }
        private void limpiar()
        {
            idMarca = 0;
            txtNombre.Text = "";
            //desmarca la fila seleccionada
            dgvMarcas.ClearSelection();
            btnEliminar.Enabled = false;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if(idMarca > 0)
            {
                //buscar por id de la marca
                //Find => SELECT * FROM Marca WHERE id_marca = idMarca;
                Marca marca = db.Marca.Find(idMarca);

                if(marca != null)
                {
                    //remueve el registro de la tabla
                    db.Marca.Remove(marca);//DELETE FROM Marca WHERE id_marca = marca
                    //guarda los cambios
                    db.SaveChanges();
                    MessageBox.Show("Eliminado con éxito!");
                    limpiar();
                    cargarMarcas();
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();  
        }

        private void FormMarca_Load(object sender, EventArgs e)
        {
            cargarColores();
            this.rvMarca.RefreshReport();
        }
    }
}
