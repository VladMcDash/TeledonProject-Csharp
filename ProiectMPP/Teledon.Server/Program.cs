using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Teledon.Networking;
// using Teledon.Persistence;

namespace Teledon.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Reading Configuration...");
            string ip = ConfigurationManager.AppSettings["ip"] ?? "127.0.0.1";
            int port = int.Parse(ConfigurationManager.AppSettings["port"] ?? "55555");
            
            string connectionString = ConfigurationManager.ConnectionStrings["teledonDB"]?.ConnectionString ?? "Data Source=teledon.db";
            connectionString = ResolveConnectionString(connectionString);
            Console.WriteLine($"DB connection: {connectionString}");
            
            var dbUtils = new TeledonProject.Repository.DbUtils(connectionString);
            
            var volunteerRepo = new ProiectMPP.TeledonProject.Repository.VolunteerDbRepository(dbUtils);
            var caseRepo = new ProiectMPP.TeledonProject.Repository.CharityCaseDbRepository(dbUtils);
            var donorRepo = new ProiectMPP.TeledonProject.Repository.DonorDbRepository(dbUtils);
            var donationRepo = new ProiectMPP.TeledonProject.Repository.DonationDbRepository(dbUtils);
            
            Console.WriteLine("Initializing Service...");
            TeledonServicesImpl serverImpl = new TeledonServicesImpl(volunteerRepo, caseRepo, donorRepo, donationRepo);

            Console.WriteLine($"Starting server on {ip}:{port}...");
            StartServer(ip, port, serverImpl);
        }

        private static void StartServer(string ip, int port, TeledonServicesImpl serverImpl)
        {
            TcpListener server = null;
            try
            {
                IPAddress adr = IPAddress.Parse(ip);
                server = new TcpListener(adr, port);
                server.Start();
                Console.WriteLine("Server started. Waiting for clients...");

                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Client connected...");
                    
                    TeledonClientRpcWorker worker = new TeledonClientRpcWorker(serverImpl, client);
                    Thread t = new Thread(worker.Run);
                    t.Start();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Server exception: " + e.Message);
            }
            finally
            {
                server?.Stop();
            }
        }

        private static string ResolveConnectionString(string connectionString)
        {
            const string dataSourceKey = "Data Source=";
            var parts = connectionString.Split(';', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < parts.Length; i++)
            {
                string part = parts[i].Trim();
                if (part.StartsWith(dataSourceKey, StringComparison.OrdinalIgnoreCase))
                {
                    string path = part.Substring(dataSourceKey.Length).Trim();
                    if (!Path.IsPathRooted(path))
                    {
                        string fullPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, path));
                        parts[i] = dataSourceKey + fullPath;
                    }
                    break;
                }
            }
            return string.Join(";", parts);
        }
    }
}