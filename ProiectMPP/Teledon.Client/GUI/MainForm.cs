using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ProiectMPP.TeledonProject.Domain;
using Teledon.Services;

namespace TeledonProject.GUI
{
    public partial class MainForm : Form, ITeledonObserver
    {
        private readonly ITeledonServices _service;
        private readonly Volunteer _currentUser;

        public MainForm(ITeledonServices service, Volunteer currentUser)
        {
            InitializeComponent();
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public void InitializeAfterLogin()
        {
            LoadCases();
        }

        private void LoadCases()
        {
            var cases = _service.GetAllCases();
            dgvCases.DataSource = cases == null ? new List<CharityCase>() : cases.ToList();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string query = txtSearch.Text;
            var results = _service.SearchDonors(query);
            
            listBoxDonors.DataSource = results?.ToList() ?? new List<Donor>();
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

                MessageBox.Show("Donatie trimisa spre inregistrare!");
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
            try
            {
                _service.Logout(_currentUser, this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Logout err: " + ex.Message);
            }
            this.Close(); 
        }
        
        private void btnUpdateDonor_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxDonors.SelectedItem is Donor selected)
                {
                    _service.UpdateDonor(
                        txtDonorName.Text, 
                        txtAddress.Text, 
                        txtPhone.Text
                    );
            
                    MessageBox.Show("Update trimis catre server!");
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

        // ITeledonObserver implementation
        public void DonationAdded(CharityCase updatedCase)
        {
            Console.WriteLine("Donation added notification received.");
            this.BeginInvoke(new Action(() => {
                LoadCases();
            }));
        }

        public void DonorUpdated(Donor updatedDonor)
        {
            Console.WriteLine("Donor updated notification received.");
            this.BeginInvoke(new Action(() => {
                txtSearch_TextChanged(null, null);
            }));
        }
    }
}