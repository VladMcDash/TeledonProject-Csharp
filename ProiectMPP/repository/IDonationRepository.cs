using ProiectMPP.domain;

namespace ProiectMPP.repository;

using System.Collections.Generic;

public interface IDonationRepository : IRepository<long, Donation> {
    IEnumerable<Donation> FindByDonor(long donorId);
}