using ProiectMPP.domain;

namespace ProiectMPP.repository;
public interface ICharityCaseRepository : IRepository<long, CharityCase> {
    void UpdateTotalAmount(long caseId, double newAmount);
}