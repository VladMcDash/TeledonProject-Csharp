using System;
using System.Windows.Forms;
using ProiectMPP.TeledonProject.Service;
using ProiectMPP.TeledonProject.Service;

namespace TeledonProject.GUI
{
    public partial class LoginForm : Form
    {
        private readonly TeledonService _service;

        public LoginForm(TeledonService service)
        {
            InitializeComponent(); 
            _service = service;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try {
                var user = _service.Login(txtUsername.Text, txtPassword.Text);
                var mainForm = new MainForm(_service);
                mainForm.Show();
                this.Hide();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
    }
}