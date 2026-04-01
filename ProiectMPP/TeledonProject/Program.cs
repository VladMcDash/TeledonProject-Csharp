using System;
using System.Configuration;
using System.Windows.Forms;
using ProiectMPP.TeledonProject.Repository;
using ProiectMPP.TeledonProject.Service;
using TeledonProject.Repository;
using ProiectMPP.TeledonProject.Service;
using TeledonProject.GUI;

namespace ProiectMPP.TeledonProject
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string connectionString = ConfigurationManager.ConnectionStrings["teledonDB"].ConnectionString;

            DbUtils dbUtils = new DbUtils(connectionString);

            try
            {
                IVolunteerRepository volunteerRepo = new VolunteerDbRepository(dbUtils);
                IDonorRepository donorRepo = new DonorDbRepository(dbUtils);
                ICharityCaseRepository caseRepo = new CharityCaseDbRepository(dbUtils);
                IDonationRepository donationRepo = new DonationDbRepository(dbUtils);

                TeledonService service = new TeledonService(volunteerRepo, donorRepo, caseRepo, donationRepo);

                Application.Run(new LoginForm(service));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare: {ex.Message}");
            }
        }
    }
}