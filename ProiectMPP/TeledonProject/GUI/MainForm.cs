using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ProiectMPP.TeledonProject.Domain;
using ProiectMPP.TeledonProject.Service;

namespace TeledonProject.GUI
{
    public partial class MainForm : Form
    {
        private readonly TeledonService _service;

        public MainForm(TeledonService service)
        {
            InitializeComponent();
            _service = service;
            LoadCases();
        }

        private void LoadCases()
        {
            dgvCases.DataSource = _service.GetAllCases().ToList();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string query = txtSearch.Text;
            var results = _service.SearchDonors(query).ToList();
            
            listBoxDonors.DataSource = results;
            listBoxDonors.DisplayMember = "Name"; 
        }

        private void listBoxDonors_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxDonors.SelectedItem is Donor selected)
            {
                txtDonorName.Text = selected.Name;
                txtAddress.Text = selected.Address;
                txtPhone.Text = selected.PhoneNumber;
            }
        }
        
        private void btnAddDonation_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvCases.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Va rugam sa selectati un caz din tabel!");
                    return;
                }

                var selectedCase = (CharityCase)dgvCases.SelectedRows[0].DataBoundItem;
                long caseId = selectedCase.Id;

                string name = txtDonorName.Text;
                string addr = txtAddress.Text;
                string phone = txtPhone.Text;
                double amount = double.Parse(txtAmount.Text);

                _service.AddDonation(name, addr, phone, caseId, amount);

                MessageBox.Show("Donatie inregistrata cu succes!");
                
                LoadCases();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare: " + ex.Message);
            }
        }

        private void ClearFields()
        {
            txtDonorName.Clear(); txtAddress.Clear(); txtPhone.Clear(); txtAmount.Clear();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }
        private void btnUpdateDonor_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxDonors.SelectedItem is Donor selected)
                {
                    _service.UpdateDonor(
                        selected.Id, 
                        txtDonorName.Text, 
                        txtAddress.Text, 
                        txtPhone.Text
                    );
            
                    MessageBox.Show("Datele donatorului au fost actualizate!");
                    txtSearch_TextChanged(null, null);
                }
                else
                {
                    MessageBox.Show("Selectati un donator din lista!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare: " + ex.Message);
            }
        }
    }
}