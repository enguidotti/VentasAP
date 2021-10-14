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
        private Random rnd;
        private int indexTemp;
        private Button botonSelect;
        public FormHome()
        {
            InitializeComponent();
            rnd = new Random();
        }

        //seleccionar los colores de fondo 
        private Color SeleccionColores()
        {
            //selecciona de manera aleatorea un índice de los colores de la lista
            int index;
            do
            {
                index = rnd.Next(Colores.ColorList.Count);
            } while (indexTemp == index);
            string color = Colores.ColorList[index];

            return ColorTranslator.FromHtml(color);
        }
        //método para asignar colores al botón presionado 
        private void BotonActivo(object btnSender)
        {
            if(btnSender != null)
            {
                BotonDesactivado();
                //se llama el método que escoge el color
                Color color = SeleccionColores();
                //se asigna a la variable el botón presionado
                botonSelect = (Button)btnSender;
                //se le da el color de fondo al botón seleccionado
                botonSelect.BackColor = color;
                //las letras de botón se asignas blancas
                botonSelect.ForeColor = Color.AntiqueWhite;
                //cambiar el color del panel superior
                panelTop.BackColor = color;

            }
        }
        private void BotonDesactivado()
        {
            //foreach para recorrer todos los elementos del menú
            foreach (Control btn in panelMenu.Controls)
            {
                //se necesita saber si el control es de tipo button
                if(btn.GetType() == typeof(Button))
                {
                    btn.BackColor = Color.FromArgb(51, 51, 76);
                    btn.ForeColor = Color.White;
                }
            }
        }
        //abrir formulario dentro del formulario FormHome
        private void abrirFormulario(Form formHijo, object btnSender)
        {
            //si existe un formulario activo hijo, lo cierra
            if (formularioActivo != null)
            {
                formularioActivo.Close();
            }
            //cada vez presiona el botón para el abrir el formulario invoca al método para cambiar colores
            BotonActivo(btnSender);
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
            abrirFormulario(new Formularios.FormCompra(),sender);
        }

        private void btnProducto_Click(object sender, EventArgs e)
        {
            abrirFormulario(new Formularios.FormProducto(), sender);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            abrirFormulario(new Formularios.FormMarca(), sender);
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            if (formularioActivo != null)
            {
                formularioActivo.Close();
            }
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            abrirFormulario(new Formularios.FormUser(), sender);
        }
    }
}
