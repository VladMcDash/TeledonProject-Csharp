using ProiectMPP.TeledonProject.Domain;

namespace Teledon.Services
{
    public interface ITeledonObserver
    {
        void DonationAdded(CharityCase updatedCase);
        void DonorUpdated(Donor updatedDonor);
    }
}