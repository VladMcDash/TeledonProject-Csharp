using ProiectMPP.TeledonProject.Domain;
using TeledonProject.repository;

namespace TeledonProject.Repository
{
    public interface ICharityCaseRepository : IRepository<long, CharityCase>
    {
        void UpdateTotalAmount(long caseId, double amountToAdd);
    }
}