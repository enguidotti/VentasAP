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
    public partial class FormUser : Form
    {
        private apventasEntities db = new apventasEntities();
        private Helpers help = new Helpers();
        public FormUser()
        {
            InitializeComponent();
            CargarRol();
            CargarGrilla();
            CargarReporte();
        }
      private void CargarReporte()
        {
            var listaUsuario = db.User.ToList();
            ReportDataSource report = new ReportDataSource("Usuario",listaUsuario);
            rvUsuario.LocalReport.DataSources.Clear();
            rvUsuario.LocalReport.DataSources.Add(report);
            rvUsuario.RefreshReport();
        }
        private void CargarRol()
        {
            var listaRol = db.Rol.ToList();

            cbRol.DataSource = listaRol;
            cbRol.ValueMember = "id_rol";
            cbRol.DisplayMember = "nombre";
            cbRol.SelectedIndex = -1;
        }
        private void CargarGrilla()
        {
            var listaUsuario = (from u in db.User
                                select new
                                {
                                    u.id_user,
                                    u.id_rol,
                                    Run = u.rut,
                                    Nombres = u.nombres,
                                    Apellidos = u.apellidos,
                                    Email = u.email,
                                    Password = u.password,
                                    Rol = u.Rol.nombre
                                }).ToList();
            dgvUser.DataSource = listaUsuario;
            dgvUser.Columns[0].Visible = false;
            dgvUser.Columns[1].Visible = false;
        }

        private string Validar()
        {
            string error = string.Empty;
            if (string.IsNullOrEmpty(cbRol.Text))
                error = "Debe seleccionar un rol \n";
            if (string.IsNullOrEmpty(txtRun.Text))
                error += "Debe ingresar un Run \n";
            if (string.IsNullOrEmpty(txtNombres.Text))
                error += "Debe ingresar Nombres \n";
            if (string.IsNullOrEmpty(txtApellidos.Text))
                error += "Debe ingresar Apellidos \n";
            if (string.IsNullOrEmpty(txtEmail.Text))
                error += "Debe ingresar un Email \n";
            if (string.IsNullOrEmpty(txtPassword.Text))
                error += "Debe ingresar una Contraseña \n";
            if (string.IsNullOrEmpty(txtRepetir.Text))
                error += "Debe repetir contraseña \n";
            if (!string.IsNullOrEmpty(txtPassword.Text) && !string.IsNullOrEmpty(txtRepetir.Text) && txtPassword.Text != txtRepetir.Text)
                error += "Las contraseñas deben ser iguales \n";

            return error;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje = Validar();
            if (mensaje != "")
            {
                MessageBox.Show(mensaje);
            }
            else
            {
                User user = new User();
                user.id_rol = int.Parse(cbRol.SelectedValue.ToString());
                user.rut = txtRun.Text;
                user.nombres = txtNombres.Text;
                user.apellidos = txtApellidos.Text;
                user.email = txtEmail.Text;
                user.password = txtPassword.Text;

                db.User.Add(user);
                db.SaveChanges();

                MessageBox.Show("Se ha guardado con éxito!!!");
            }


        }

        private void txtEmail_Leave(object sender, EventArgs e)
        {
            if (txtEmail.Text.Trim() != "")
            {
                if (!help.emailValido(txtEmail.Text.Trim()))
                {
                    MessageBox.Show("El email no tiene el formato corrcto (mail@mail.com)");
                }
            }
        }

        private void txtRun_TextChanged(object sender, EventArgs e)
        {
            if (txtRun.Text.Trim() != "")
            {
                txtRun.Text = help.formatearRut(txtRun.Text.Trim());
                //poner el cursor siempre al final del texto
                txtRun.Select(txtRun.Text.Length, 0);
            }
        }

        private void txtRun_Leave(object sender, EventArgs e)
        {
            if (txtRun.Text.Trim() != string.Empty)
            {
                if (!help.validarRut(txtRun.Text.Trim()))
                {
                    MessageBox.Show("El Run ingresado no es válido!");
                    txtRun.Focus();
                }
            }
        }
        private void cargarColores()
        {
            //recorre todas los controles o herramientas que hay en el formulario
            foreach (Control btns in Controls)
            {
                //verifica que los controles sean del tipo Button(que sea un botón)
                if (btns.GetType() == typeof(Button))
                {
                    Button btn = (Button)btns;
                    //COlores es la clase con la lista de colores
                    btn.BackColor = Colores.PrimaryColor;
                    btn.ForeColor = Color.White;
                    btn.FlatAppearance.BorderColor = Colores.SecondaryColor;
                }
                lblTitulo.ForeColor = Colores.SecondaryColor;
                dgvUser.ColumnHeadersDefaultCellStyle.BackColor = Colores.PrimaryColor;
                dgvUser.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvUser.EnableHeadersVisualStyles = false;

                dgvUser.Font = new Font("Times", 14);

            }
        }
        private void FormUser_Load(object sender, EventArgs e)
        {
            cargarColores();
            this.rvUsuario.RefreshReport();
        }
    }
}
