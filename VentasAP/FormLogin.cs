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

namespace VentasAP
{
    public partial class FormLogin : Form
    {
        private apventasEntities db = new apventasEntities();
        private Helpers help = new Helpers();
        //variables static que serán accedidas desde otros formularios
        public static int id_user;
        public static string nombre_user;
        public static int id_rol;
        public FormLogin()
        {
            InitializeComponent();
            // ControlBox = false;
            //MaximizedBounds = Screen.FromHandle(Handle).WorkingArea;
        }

        private void btnIngreso_Click(object sender, EventArgs e)
        {
            string error = "";
            if (string.IsNullOrEmpty(txtEmail.Text.Trim()))
                error = "Debe ingresar un email \n";
            if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
                error += "Debe ingresar una contraseña";
            if (error != "")
                MessageBox.Show(error);
            else
            {
                //consulta para verficar existencia del usuario 
                User user = db.User.FirstOrDefault(u => u.email.Equals(txtEmail.Text.Trim()) && u.password.Equals(txtPassword.Text));
                if (user != null)
                {
                    //una vez logeado se asignan los valores a las variables statics
                    id_user = user.id_user;
                    nombre_user = user.nombres + " " + user.apellidos;
                    id_rol = user.id_rol;

                    FormHome home = new FormHome();
                    home.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Email o Contraseña no son correctos");
                }
            }
        }

        private void txtEmail_Leave(object sender, EventArgs e)
        {
            if (txtEmail.Text.Trim() != string.Empty)
            {
                if (!help.emailValido(txtEmail.Text.Trim()))
                {
                    MessageBox.Show("El email no tiene el formato corrcto (mail@mail.com)");
                }
            }
        }
    }
}
