using System.Collections.Generic;
using ProiectMPP.TeledonProject.Domain;
using TeledonProject.repository;

namespace TeledonProject.Repository
{
    public interface IDonationRepository : IRepository<long, Donation>
    {
        IEnumerable<Donation> FindByDonor(long donorId);
    }
}