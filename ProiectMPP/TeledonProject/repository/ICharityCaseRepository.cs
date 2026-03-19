using ProiectMPP.TeledonProject.domain;
using TeledonProject.repository;

namespace TeledonProject.Repository
{
    public interface ICharityCaseRepository : IRepository<long, CharityCase>
    {
        void UpdateTotalAmount(long caseId, double amountToAdd);
    }
}