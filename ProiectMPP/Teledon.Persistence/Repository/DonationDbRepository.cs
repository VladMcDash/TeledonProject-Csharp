using System;
using System.Collections.Generic;
using System.Data; 
using Microsoft.Data.Sqlite;
using ProiectMPP.TeledonProject.Domain;
using ProiectMPP.TeledonProject.Repository;
using TeledonProject.Repository;

namespace ProiectMPP.TeledonProject.Repository
{
    public class DonationDbRepository : IDonationRepository
    {
        private readonly DbUtils _dbUtils;

        public DonationDbRepository(DbUtils dbUtils)
        {
            _dbUtils = dbUtils;
        }

        public void Add(Donation entity)
        {
            using (var con = (SqliteConnection)_dbUtils.GetConnection())
            {
                con.Open();
                using (SqliteCommand cmd = con.CreateCommand()) 
                {
                    cmd.CommandText = "INSERT INTO Donations (donor_id, case_id, amount) VALUES (@donor, @case, @amount)";
        
                    cmd.Parameters.AddWithValue("@donor", entity.Donor.Id);
                    cmd.Parameters.AddWithValue("@case", entity.CharityCase.Id);
                    cmd.Parameters.AddWithValue("@amount", entity.Amount);
        
                    cmd.ExecuteNonQuery();
                }
            } 
        }

        public IEnumerable<Donation> FindByDonor(long donorId)
        {
            return new List<Donation>();
        }

        public IEnumerable<Donation> FindAll() => new List<Donation>();
        public Donation FindOne(long id) => null;
        public void Update(long id, Donation entity) { }
        public void Delete(long id) { }
    }
}