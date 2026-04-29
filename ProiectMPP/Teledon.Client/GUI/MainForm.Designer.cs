namespace TeledonProject.GUI
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        
        // Componente UI
        private System.Windows.Forms.DataGridView dgvCases;
        private System.Windows.Forms.ListBox listBoxDonors;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.TextBox txtDonorName;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.TextBox txtAmount;
        private System.Windows.Forms.Button btnAddDonation;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Label lblCazuri;
        private System.Windows.Forms.Label lblDonatori;
        private System.Windows.Forms.Button btnUpdateDonor;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnUpdateDonor = new System.Windows.Forms.Button();

            this.btnUpdateDonor.Text = "Actualizeaza Date Donator";
            this.btnUpdateDonor.Location = new System.Drawing.Point(450, 360); 
            this.btnUpdateDonor.Size = new System.Drawing.Size(250, 35);
            this.btnUpdateDonor.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnUpdateDonor.Click += new System.EventHandler(this.btnUpdateDonor_Click);

            this.Controls.Add(this.btnUpdateDonor);
            dgvCases = new System.Windows.Forms.DataGridView();
            listBoxDonors = new System.Windows.Forms.ListBox();
            txtSearch = new System.Windows.Forms.TextBox();
            txtDonorName = new System.Windows.Forms.TextBox();
            txtAddress = new System.Windows.Forms.TextBox();
            txtPhone = new System.Windows.Forms.TextBox();
            txtAmount = new System.Windows.Forms.TextBox();
            btnAddDonation = new System.Windows.Forms.Button();
            btnLogout = new System.Windows.Forms.Button();
            lblCazuri = new System.Windows.Forms.Label();
            lblDonatori = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)dgvCases).BeginInit();
            SuspendLayout();
            // 
            // dgvCases
            // 
            dgvCases.ColumnHeadersHeight = 29;
            dgvCases.Location = new System.Drawing.Point(20, 40);
            dgvCases.MultiSelect = false;
            dgvCases.Name = "dgvCases";
            dgvCases.RowHeadersWidth = 51;
            dgvCases.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvCases.Size = new System.Drawing.Size(400, 300);
            dgvCases.TabIndex = 0;
            // 
            // listBoxDonors
            // 
            listBoxDonors.Location = new System.Drawing.Point(450, 70);
            listBoxDonors.Name = "listBoxDonors";
            listBoxDonors.Size = new System.Drawing.Size(250, 84);
            listBoxDonors.TabIndex = 1;
            listBoxDonors.SelectedIndexChanged += listBoxDonors_SelectedIndexChanged;
            // 
            // txtSearch
            // 
            txtSearch.Location = new System.Drawing.Point(450, 40);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Cauta donator...";
            txtSearch.Size = new System.Drawing.Size(250, 27);
            txtSearch.TabIndex = 2;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // txtDonorName
            // 
            txtDonorName.Location = new System.Drawing.Point(450, 180);
            txtDonorName.Name = "txtDonorName";
            txtDonorName.PlaceholderText = "Nume Donator";
            txtDonorName.Size = new System.Drawing.Size(250, 27);
            txtDonorName.TabIndex = 3;
            // 
            // txtAddress
            // 
            txtAddress.Location = new System.Drawing.Point(450, 210);
            txtAddress.Name = "txtAddress";
            txtAddress.PlaceholderText = "Adresa";
            txtAddress.Size = new System.Drawing.Size(250, 27);
            txtAddress.TabIndex = 4;
            // 
            // txtPhone
            // 
            txtPhone.Location = new System.Drawing.Point(450, 240);
            txtPhone.Name = "txtPhone";
            txtPhone.PlaceholderText = "Telefon";
            txtPhone.Size = new System.Drawing.Size(250, 27);
            txtPhone.TabIndex = 5;
            // 
            // txtAmount
            // 
            txtAmount.Location = new System.Drawing.Point(450, 270);
            txtAmount.Name = "txtAmount";
            txtAmount.PlaceholderText = "Suma (RON)";
            txtAmount.Size = new System.Drawing.Size(250, 27);
            txtAmount.TabIndex = 6;
            // 
            // btnAddDonation
            // 
            btnAddDonation.BackColor = System.Drawing.Color.LightGreen;
            btnAddDonation.Location = new System.Drawing.Point(450, 310);
            btnAddDonation.Name = "btnAddDonation";
            btnAddDonation.Size = new System.Drawing.Size(250, 40);
            btnAddDonation.TabIndex = 7;
            btnAddDonation.Text = "Inregistreaza Donatie";
            btnAddDonation.UseVisualStyleBackColor = false;
            btnAddDonation.Click += btnAddDonation_Click;
            // 
            // btnLogout
            // 
            btnLogout.Location = new System.Drawing.Point(20, 350);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new System.Drawing.Size(83, 38);
            btnLogout.TabIndex = 8;
            btnLogout.Text = "Logout";
            btnLogout.Click += btnLogout_Click;
            // 
            // lblCazuri
            // 
            lblCazuri.Location = new System.Drawing.Point(0, 0);
            lblCazuri.Name = "lblCazuri";
            lblCazuri.Size = new System.Drawing.Size(100, 23);
            lblCazuri.TabIndex = 0;
            // 
            // lblDonatori
            // 
            lblDonatori.Location = new System.Drawing.Point(0, 0);
            lblDonatori.Name = "lblDonatori";
            lblDonatori.Size = new System.Drawing.Size(100, 23);
            lblDonatori.TabIndex = 0;
            // 
            // MainForm
            // 
            ClientSize = new System.Drawing.Size(730, 400);
            Controls.Add(dgvCases);
            Controls.Add(listBoxDonors);
            Controls.Add(txtSearch);
            Controls.Add(txtDonorName);
            Controls.Add(txtAddress);
            Controls.Add(txtPhone);
            Controls.Add(txtAmount);
            Controls.Add(btnAddDonation);
            Controls.Add(btnLogout);
            Text = "Panou Gestiune Teledon";
            ((System.ComponentModel.ISupportInitialize)dgvCases).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}