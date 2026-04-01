using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using ProiectMPP.TeledonProject.Domain;
using TeledonProject.Repository;

namespace ProiectMPP.TeledonProject.Repository
{
    public class DonorDbRepository : IDonorRepository
    {
        private readonly DbUtils _dbUtils;

        public DonorDbRepository(DbUtils dbUtils)
        {
            _dbUtils = dbUtils;
        }

        public IEnumerable<Donor> FindByNameLike(string namePart)
        {
            IList<Donor> donors = new List<Donor>();
            using (var con = (SqliteConnection)_dbUtils.GetConnection())
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM Donors WHERE name LIKE @name";
                    cmd.Parameters.AddWithValue("@name", "%" + namePart + "%");
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            donors.Add(new Donor(reader.GetString(1), reader.GetString(2), reader.GetString(3)) { Id = reader.GetInt64(0) });
                        }
                    }
                }
            }
            return donors;
        }

        public Donor FindByName(string name)
        {
            using (var con = (SqliteConnection)_dbUtils.GetConnection())
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM Donors WHERE name = @name";
                    cmd.Parameters.AddWithValue("@name", name);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return new Donor(reader.GetString(1), reader.GetString(2), reader.GetString(3)) { Id = reader.GetInt64(0) };
                    }
                }
            }
            return null;
        }

        public void Add(Donor entity)
        {
            using (var con = (SqliteConnection)_dbUtils.GetConnection())
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Donors (name, address, phone) VALUES (@n, @a, @p)";
                    cmd.Parameters.AddWithValue("@n", entity.Name);
                    cmd.Parameters.AddWithValue("@a", entity.Address);
                    cmd.Parameters.AddWithValue("@p", entity.PhoneNumber);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(long id, Donor entity) { }
        public void Delete(long id) { }
        public Donor FindOne(long id) => null;
        public IEnumerable<Donor> FindAll() => new List<Donor>();
    }
}