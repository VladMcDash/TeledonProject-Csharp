using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using ProiectMPP.TeledonProject.Domain;
using TeledonProject.Repository;

namespace ProiectMPP.TeledonProject.Repository
{
    public class CharityCaseDbRepository : ICharityCaseRepository
    {
        private readonly DbUtils _dbUtils;

        public CharityCaseDbRepository(DbUtils dbUtils)
        {
            _dbUtils = dbUtils;
        }

        public IEnumerable<CharityCase> FindAll()
        {
            IList<CharityCase> cases = new List<CharityCase>();
            using (var con = (SqliteConnection)_dbUtils.GetConnection())
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM CharityCases";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var c = new CharityCase {
                                Id = reader.GetInt64(0),
                                Name = reader.GetString(1),
                                TotalAmount = reader.GetDouble(2)
                            };
                            cases.Add(c);
                        }
                    }
                }
            }
            return cases;
        }

        public void UpdateTotalAmount(long caseId, double amount)
        {
            using (var con = (SqliteConnection)_dbUtils.GetConnection())
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "UPDATE CharityCases SET totalAmount = totalAmount + @amount WHERE id = @id";
                    cmd.Parameters.AddWithValue("@amount", amount);
                    cmd.Parameters.AddWithValue("@id", caseId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Add(CharityCase entity) { }
        public void Delete(long id) { }
        public void Update(long id, CharityCase entity) { }
        public CharityCase FindOne(long id) => null;
    }
}