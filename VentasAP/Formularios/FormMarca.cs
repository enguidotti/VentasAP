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
        public FormMarca()
        {
            InitializeComponent();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (txtNombre.Text != "")
            {
                //instancia la clase marca
                Marca marca = new Marca();
                //asigno al atributo nombre el texto ingresado desde el formulario
                marca.nombre = txtNombre.Text.Trim();  //.Trim() para todos los lenguaje y quita los espacios en blanco
                                                       //se añade el registro a la base de dato
                db.Marca.Add(marca);//INSERT INTO Marca(nombre) values('Apple');
                                    //guardar los cambios realizados a la base de datos
                db.SaveChanges();
            } 
            else
            {
                MessageBox.Show("Debe ingrese un nombre de marca");
            }
        }
    }
}
