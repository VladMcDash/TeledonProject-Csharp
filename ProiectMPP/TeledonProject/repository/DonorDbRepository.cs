using System.Collections.Generic;
using System.Data;
using ProiectMPP.TeledonProject.domain;
using log4net;
using ProiectMPP.TeledonProject.Repository;

namespace TeledonProject.Repository
{
    public class DonorDbRepository : IDonorRepository
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DonorDbRepository));
        private readonly DbUtils _dbUtils;

        public DonorDbRepository(DbUtils dbUtils)
        {
            Log.Info("Initializing DonorDbRepository");
            _dbUtils = dbUtils;
        }

        public void Add(Donor entity)
        {
            Log.DebugFormat("Entering Add with entity: {0}", entity);
            IDbConnection con = _dbUtils.GetConnection();
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "INSERT INTO Donors (name, address, phoneNumber) VALUES (@name, @address, @phone)";
                
                var paramName = comm.CreateParameter();
                paramName.ParameterName = "@name";
                paramName.Value = entity.Name;
                comm.Parameters.Add(paramName);

                var paramAddr = comm.CreateParameter();
                paramAddr.ParameterName = "@address";
                paramAddr.Value = entity.Address;
                comm.Parameters.Add(paramAddr);

                var paramPhone = comm.CreateParameter();
                paramPhone.ParameterName = "@phone";
                paramPhone.Value = entity.PhoneNumber;
                comm.Parameters.Add(paramPhone);

                comm.ExecuteNonQuery();
            }
            Log.Debug("Exiting Add");
        }

        public IEnumerable<Donor> FindByNameLike(string namePart)
        {
            Log.DebugFormat("Entering FindByNameLike with param: {0}", namePart);
            IDbConnection con = _dbUtils.GetConnection();
            IList<Donor> donors = new List<Donor>();

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT * FROM Donors WHERE name LIKE @namePart";
                var param = comm.CreateParameter();
                param.ParameterName = "@namePart";
                param.Value = "%" + namePart + "%";
                comm.Parameters.Add(param);

                using (var dataR = comm.ExecuteReader())
                {
                    while (dataR.Read())
                    {
                        Donor d = new Donor(dataR.GetString(1), dataR.GetString(2), dataR.GetString(3))
                        {
                            Id = dataR.GetInt64(0)
                        };
                        donors.Add(d);
                    }
                }
            }
            Log.DebugFormat("Exiting FindByNameLike with {0} results", donors.Count);
            return donors;
        }

        public Donor FindOne(long id) { return null; }
        public IEnumerable<Donor> FindAll() { return new List<Donor>(); }
        public void Delete(long id) { }
        public void Update(long id, Donor entity) { }
        public Donor FindByName(string name) { return null; }
    }
}