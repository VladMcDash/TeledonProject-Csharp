using ProiectMPP.TeledonProject.Domain;

namespace Teledon.Services
{
    public interface ITeledonServices
    {
        void Login(Volunteer volunteer, ITeledonObserver client);
        void Logout(Volunteer volunteer, ITeledonObserver client);
        List<CharityCase> GetAllCases();
        void AddDonation(string name, string address, string phone, long caseId, double amount);
        List<Donor> SearchDonors(string namePart);
        void UpdateDonor(string name, string address, string phone);
    }
}