using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using ProiectMPP.TeledonProject.Domain;
using TeledonProject.Repository;

namespace ProiectMPP.TeledonProject.Repository
{
    public class VolunteerDbRepository : IVolunteerRepository
    {
        private readonly DbUtils _dbUtils;

        public VolunteerDbRepository(DbUtils dbUtils)
        {
            _dbUtils = dbUtils;
        }

        public Volunteer FindByUsernameAndPassword(string username, string password)
        {
            using (var con = (SqliteConnection)_dbUtils.GetConnection()) 
            {
                con.Open();
                using (SqliteCommand cmd = con.CreateCommand()) 
                {
                    cmd.CommandText = "SELECT * FROM Volunteers WHERE username = @user AND password = @pass";
                    cmd.Parameters.AddWithValue("@user", username);
                    cmd.Parameters.AddWithValue("@pass", password);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int idIdx = reader.GetOrdinal("id");
                            int userIdx = reader.GetOrdinal("username");
                            int passIdx = reader.GetOrdinal("password");
                            int nameIdx = reader.GetOrdinal("name");

                            var v = new Volunteer(
                                reader.GetString(userIdx),
                                reader.GetString(passIdx),
                                reader.GetString(nameIdx)
                            );
                            v.Id = reader.GetInt64(idIdx);
                            return v;
                        }
                    }
                }
            }
            return null;
        }

// Nu uita sa adaugi si restul metodelor (Add, Delete, Update, FindAll, FindOne) chiar daca sunt goale!

        // Metode obligatorii din interfață (dacă lipsesc, va fi eroare roșie pe numele clasei)
        public IEnumerable<Volunteer> FindAll() => new List<Volunteer>();
        public Volunteer FindOne(long id) => null;
        public void Add(Volunteer entity) { }
        public void Update(long id, Volunteer entity) { }
        public void Delete(long id) { }
    }
}