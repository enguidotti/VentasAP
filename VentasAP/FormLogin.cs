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
        public static int id_user;//primera forma
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
                if(user != null)
                {
                    id_user = user.id_user;
                    if (user.id_rol == 1)
                    {
                        FormHome home = new FormHome();
                        home.Show();
                        this.Hide();
                    }
                }
                else
                {
                    MessageBox.Show("Email o Contraseña no son correctos");
                }
            }
        }
    }
}
