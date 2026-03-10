using ProiectMPP.domain;

namespace ProiectMPP.repository;

using System.Collections.Generic;

public interface IDonorRepository : IRepository<long, Donor> {
    IEnumerable<Donor> FindByNameLike(string namePart);
    Donor FindByName(string name);
}