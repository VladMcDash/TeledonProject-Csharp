using ProiectMPP.domain;

namespace ProiectMPP.repository;

public interface IVolunteerRepository : IRepository<long, Volunteer> {
    Volunteer FindByUsernameAndPassword(string username, string password);
}