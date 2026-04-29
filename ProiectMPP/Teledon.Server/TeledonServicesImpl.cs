using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ProiectMPP.TeledonProject.Domain;
using Teledon.Services;
using TeledonProject.Repository;

namespace Teledon.Server
{
    public class TeledonServicesImpl : ITeledonServices
    {
        private IVolunteerRepository volunteerRepo;
        private ICharityCaseRepository caseRepo;
        private IDonorRepository donorRepo;
        private IDonationRepository donationRepo;

        private readonly ConcurrentDictionary<string, ITeledonObserver> loggedClients;

        public TeledonServicesImpl(IVolunteerRepository volunteerRepo, ICharityCaseRepository caseRepo, IDonorRepository donorRepo, IDonationRepository donationRepo)
        {
            this.volunteerRepo = volunteerRepo;
            this.caseRepo = caseRepo;
            this.donorRepo = donorRepo;
            this.donationRepo = donationRepo;
            loggedClients = new ConcurrentDictionary<string, ITeledonObserver>();
        }

        public void Login(Volunteer volunteer, ITeledonObserver client)
        {
            string hashed = HashPassword(volunteer.Password);
            Volunteer validUser = volunteerRepo.FindByUsernameAndPassword(volunteer.Username, hashed);
            if (validUser == null)
            {
                 throw new Exception("Authentication failed.");
            }
            
            if (loggedClients.ContainsKey(validUser.Id.ToString()))
            {
                throw new Exception("Volunteer is already logged in.");
            }
            loggedClients[validUser.Id.ToString()] = client;
            
            // Put the valid ID into the volunteer so that Logout works
            volunteer.Id = validUser.Id;
        }

        private static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        public void Logout(Volunteer volunteer, ITeledonObserver client)
        {
            ITeledonObserver removedClient;
            bool removed = loggedClients.TryRemove(volunteer.Id.ToString(), out removedClient);
            if (!removed)
            {
                throw new Exception("Volunteer is not logged in.");
            }
        }

        public List<CharityCase> GetAllCases()
        {
            return caseRepo.FindAll().ToList();
        }

        public void AddDonation(string name, string address, string phone, long caseId, double amount)
        {
            var allDonors = donorRepo.FindAll();
            Donor existingDonor = allDonors.FirstOrDefault(d => d.Name == name && d.PhoneNumber == phone);
            
            if (existingDonor == null)
            {
                existingDonor = new Donor(name, address, phone);
                donorRepo.Add(existingDonor);
                // Assume Add updates the ID, if not, find it again.
                // SQLite returns last_insert_rowid() usually.
                // Let's do a quick FindByName since DonorDbRepository implements it
                existingDonor = donorRepo.FindByName(name);
            }

            CharityCase cc = caseRepo.FindOne(caseId);
            if (cc == null) throw new Exception("Charity case not found");

            Donation donation = new Donation(existingDonor, cc, amount);
            donationRepo.Add(donation);

            cc.TotalAmount += amount;
            caseRepo.UpdateTotalAmount(cc.Id, amount);

            NotifyDonationAdded(cc);
        }

        public List<Donor> SearchDonors(string namePart)
        {
            var donors = donorRepo.FindAll();
            return donors.Where(d => d.Name.ToLower().Contains(namePart.ToLower())).ToList();
        }

        public void UpdateDonor(string name, string address, string phone)
        {
            var donors = donorRepo.FindAll();
            Donor donorToUpdate = donors.FirstOrDefault(d => d.Name == name);
            if (donorToUpdate != null)
            {
                donorToUpdate.Address = address;
                donorToUpdate.PhoneNumber = phone;
                donorRepo.Update(donorToUpdate.Id, donorToUpdate);
                NotifyDonorUpdated(donorToUpdate);
            }
            else
            {
                throw new Exception("Donor not found");
            }
        }

        private void NotifyDonationAdded(CharityCase updatedCase)
        {
            foreach (var client in loggedClients.Values)
            {
                Task.Run(() =>
                {
                    try
                    {
                        client.DonationAdded(updatedCase);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error notifying client: " + e.Message);
                    }
                });
            }
        }

        private void NotifyDonorUpdated(Donor updatedDonor)
        {
            foreach (var client in loggedClients.Values)
            {
                Task.Run(() =>
                {
                    try
                    {
                        client.DonorUpdated(updatedDonor);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error notifying client: " + e.Message);
                    }
                });
            }
        }
    }
}
