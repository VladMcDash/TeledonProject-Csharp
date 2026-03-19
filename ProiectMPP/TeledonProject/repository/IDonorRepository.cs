using System.Collections.Generic;
using ProiectMPP.TeledonProject.domain;
using TeledonProject.repository;

namespace TeledonProject.Repository
{
    public interface IDonorRepository : IRepository<long, Donor>
    {
        IEnumerable<Donor> FindByNameLike(string namePart);

        Donor FindByName(string name);
    }
}