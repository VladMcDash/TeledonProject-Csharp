using ProiectMPP.TeledonProject.domain;
using TeledonProject.repository;

namespace TeledonProject.Repository
{
    public interface IVolunteerRepository : IRepository<long, Volunteer>
    {
        Volunteer FindByUsernameAndPassword(string username, string password);
    }
}