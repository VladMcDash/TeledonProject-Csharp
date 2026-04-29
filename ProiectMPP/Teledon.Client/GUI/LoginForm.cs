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
                // login is called from mainForm or we call it here and pass the observer?
                // The prompt says "MainForm trebuie să implementeze interfața ITeledonObserver."
                // So login should be called inside MainForm, or we call it here and pass mainForm.
                _service.Login(v, mainForm);
                mainForm.Show();
                this.Hide();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
    }
}