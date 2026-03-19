using System.Collections.Generic;
using System.Data;
using ProiectMPP.TeledonProject.domain;
using log4net;
using ProiectMPP.TeledonProject.Repository;

namespace TeledonProject.Repository
{
    public class CharityCaseDbRepository : ICharityCaseRepository
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(CharityCaseDbRepository));
        private readonly DbUtils _dbUtils;

        public CharityCaseDbRepository(DbUtils dbUtils) => _dbUtils = dbUtils;

        public IEnumerable<CharityCase> FindAll()
        {
            Log.Debug("Entering FindAll CharityCases");
            IDbConnection con = _dbUtils.GetConnection();
            IList<CharityCase> cases = new List<CharityCase>();

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT * FROM CharityCases";
                using (var dataR = comm.ExecuteReader())
                {
                    while (dataR.Read())
                    {
                        CharityCase c = new CharityCase(dataR.GetString(1), dataR.GetDouble(2))
                        {
                            Id = dataR.GetInt64(0)
                        };
                        cases.Add(c);
                    }
                }
            }
            return cases;
        }

        public void UpdateTotalAmount(long caseId, double amountToAdd)
        {
            Log.DebugFormat("Updating amount for case {0}", caseId);
            IDbConnection con = _dbUtils.GetConnection();
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "UPDATE CharityCases SET totalAmount = totalAmount + @amount WHERE id = @id";
                
                var pAmount = comm.CreateParameter();
                pAmount.ParameterName = "@amount"; pAmount.Value = amountToAdd;
                comm.Parameters.Add(pAmount);

                var pId = comm.CreateParameter();
                pId.ParameterName = "@id"; pId.Value = caseId;
                comm.Parameters.Add(pId);

                comm.ExecuteNonQuery();
            }
        }

        public void Add(CharityCase entity) { }
        public void Delete(long id) { }
        public void Update(long id, CharityCase entity) { }
        public CharityCase FindOne(long id) { return null; }
    }
}