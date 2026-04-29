using System;
using System.Windows.Forms;
using Teledon.Services;
using ProiectMPP.TeledonProject.Domain;

namespace TeledonProject.GUI
{
    public partial class LoginForm : Form
    {
        private readonly ITeledonServices _service;

        public LoginForm(ITeledonServices service)
        {
            InitializeComponent(); 
            _service = service;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try {
                // We create a dummy volunteer for the login credentials
                Volunteer v = new Volunteer(txtUsername.Text, txtPassword.Text, "") { Id = 1 };
                var mainForm = new MainForm(_service, v);
                mainForm.Show();
                this.Hide();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
    }
}