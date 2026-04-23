using System.Security.Cryptography;
using System.Text;
using ProiectMPP.TeledonProject.Domain;
using TeledonProject.Repository;
namespace ProiectMPP.TeledonProject.Service
{
    public class TeledonService
    {
        private readonly IVolunteerRepository _volunteerRepo;
        private readonly IDonorRepository _donorRepo;
        private readonly ICharityCaseRepository _caseRepo;
        private readonly IDonationRepository _donationRepo;

        public TeledonService(IVolunteerRepository vRepo, IDonorRepository dRepo, 
                              ICharityCaseRepository cRepo, IDonationRepository dnRepo)
        {
            _volunteerRepo = vRepo;
            _donorRepo = dRepo;
            _caseRepo = cRepo;
            _donationRepo = dnRepo;
        }

        public Volunteer Login(string username, string password)
        {
            string hashed = HashPassword(password);
            var user = _volunteerRepo.FindByUsernameAndPassword(username, hashed);
            if (user == null) throw new Exception("Credentiale invalide!");
            return user;
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        public IEnumerable<CharityCase> GetAllCases() => _caseRepo.FindAll();

        public IEnumerable<Donor> SearchDonors(string namePart) => _donorRepo.FindByNameLike(namePart);

        public void AddDonation(string name, string address, string phone, long caseId, double amount)
        {
            var donor = _donorRepo.FindByName(name);
            if (donor == null)
            {
                donor = new Donor(name, address, phone);
                _donorRepo.Add(donor);
                donor = _donorRepo.FindByName(name);
            }
            else if (donor.Address != address || donor.PhoneNumber != phone)
            {
                donor.Address = address;
                donor.PhoneNumber = phone;
                _donorRepo.Update(donor.Id, donor);
            }

            _donationRepo.Add(new Donation(donor, new CharityCase { Id = caseId }, amount));
            _caseRepo.UpdateTotalAmount(caseId, amount);
        }
        public void UpdateDonor(long id, string name, string address, string phoneNumber)
        {
            if (string.IsNullOrEmpty(name)) throw new Exception("Nume empty");
    
            Donor d = new Donor(name, address, phoneNumber);
            d.Id = id;
            _donorRepo.Update(id, d);
        }
    }
}//loguri pt servicii