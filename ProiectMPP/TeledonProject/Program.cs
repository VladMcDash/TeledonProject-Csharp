using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using log4net.Config;
using ProiectMPP.TeledonProject.domain;
using ProiectMPP.TeledonProject.Repository;
using TeledonProject.Repository;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]

namespace TeledonProject
{
    class Program
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {

            try{
                DbUtils dbUtils = new DbUtils();
                ICharityCaseRepository caseRepo = new CharityCaseDbRepository(dbUtils);
                IDonorRepository donorRepo = new DonorDbRepository(dbUtils);

                Console.WriteLine("cazuri caritabile:");
                
                IEnumerable<CharityCase> cases = caseRepo.FindAll();
                foreach (var c in cases)
                {
                    Console.WriteLine($"ID: {c.Id}, Nume: {c.Name}, Suma: {c.TotalAmount} RON");
                }

                string searchName = "Mihai";
                Log.Info($"Se cauta donatori cu filtrul: {searchName}");

                IEnumerable<Donor> foundDonors = donorRepo.FindByNameLike(searchName);
                foreach (var d in foundDonors)
                {
                    Console.WriteLine($"ID: {d.Id} | Nume: {d.Name} | Adresa: {d.Address} | Tel: {d.PhoneNumber}");
                }

                Donor newDonor = new Donor("Test", "Str. Rider 2026", "0799888777");
                donorRepo.Add(newDonor);
                Log.Info("Donator adaugat.");

                if (cases.GetEnumerator().MoveNext())
                {
                    long firstCaseId = 1;
                    Console.WriteLine($"\nActualizare suma pentru cazul cu ID {firstCaseId}...");
                    caseRepo.UpdateTotalAmount(firstCaseId, 100.0);
                    Log.Info($"Suma cazului {firstCaseId} a fost actualizata.");
                }

            }
            catch (Exception ex)
            {
                Log.Error("err:", ex);
                Console.WriteLine($"EROARE: {ex.Message}");
            }
            
        }
    }
}