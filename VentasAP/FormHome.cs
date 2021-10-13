using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VentasAP
{
    public partial class FormHome : Form
    {
        private Form formularioActivo;
        public FormHome()
        {
            InitializeComponent();
        }
        //abrir formulario dentro del formulario FormHome
        private void abrirFormulario(Form formHijo)
        {
            //si existe un formulario activo hijo, lo cierra
            if(formularioActivo != null)
            {
                formularioActivo.Close();
            }
            //se asigna el formulario hijo, al formulario que estará activo
            formularioActivo = formHijo;
            //se añaden propiedades al formulario hijo
            formHijo.TopLevel = false;
            formHijo.FormBorderStyle = FormBorderStyle.None;
            formHijo.Dock = DockStyle.Fill;
            //se muestra el formulario hijo dentro del panelContent
            panelContent.Controls.Add(formHijo);
            panelContent.Tag = formHijo;
            formHijo.BringToFront();
            formHijo.Show();
        }

        private void btnOrden_Click(object sender, EventArgs e)
        {
            abrirFormulario(new Formularios.FormCompra());
        }

        private void btnProducto_Click(object sender, EventArgs e)
        {
            abrirFormulario(new Formularios.FormProducto());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            abrirFormulario(new Formularios.FormMarca());
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            if(formularioActivo != null)
            {
                formularioActivo.Close();
            }
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            abrirFormulario(new Formularios.FormUser());
        }
    }
}
