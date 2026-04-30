using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading;
using ProiectMPP.TeledonProject.Domain;
using Teledon.Services;

namespace Teledon.Networking
{
    public class TeledonServerRpcProxy : ITeledonServices
    {
        private string host;
        private int port;
        private ITeledonObserver client;
        private NetworkStream stream;
        private StreamReader input;
        private StreamWriter output;
        private TcpClient connection;
        private Queue<Response> responses;
        private volatile bool finished;
        private EventWaitHandle waitHandle;

        public TeledonServerRpcProxy(string host, int port)
        {
            this.host = host;
            this.port = port;
            responses = new Queue<Response>();
        }

        public virtual void Login(Volunteer volunteer, ITeledonObserver client)
        {
            InitializeConnection();
            var req = new Request { Type = RequestType.LOGIN };
            req.SetData(volunteer);
            SendRequest(req);
            Response response = ReadResponse();
            if (response.Type == ResponseType.OK)
            {
                var loggedIn = response.GetData<Volunteer>();
                if (loggedIn != null)
                {
                    volunteer.Id = loggedIn.Id;
                }
                this.client = client;
                return;
            }
            if (response.Type == ResponseType.ERROR)
            {
                CloseConnection();
                throw new Exception(response.ErrorMessage);
            }
        }

        public virtual void Logout(Volunteer volunteer, ITeledonObserver client)
        {
            var req = new Request { Type = RequestType.LOGOUT };
            req.SetData(volunteer);
            SendRequest(req);
            Response response = ReadResponse();
            CloseConnection();
            if (response.Type == ResponseType.ERROR)
            {
                throw new Exception(response.ErrorMessage);
            }
        }

        public virtual List<CharityCase> GetAllCases()
        {
            var req = new Request { Type = RequestType.GET_CASES };
            SendRequest(req);
            Response response = ReadResponse();
            if (response.Type == ResponseType.ERROR)
            {
                throw new Exception(response.ErrorMessage);
            }
            return response.GetData<List<CharityCase>>();
        }

        public virtual void AddDonation(string name, string address, string phone, long caseId, double amount)
        {
            var req = new Request { Type = RequestType.ADD_DONATION };
            var data = new { Name = name, Address = address, Phone = phone, CaseId = caseId, Amount = amount };
            req.SetData(data);
            SendRequest(req);
            Response response = ReadResponse();
            if (response.Type == ResponseType.ERROR)
            {
                throw new Exception(response.ErrorMessage);
            }
        }

        public virtual List<Donor> SearchDonors(string namePart)
        {
            var req = new Request { Type = RequestType.SEARCH_DONORS };
            req.SetData(namePart);
            SendRequest(req);
            Response response = ReadResponse();
            if (response.Type == ResponseType.ERROR)
            {
                throw new Exception(response.ErrorMessage);
            }
            return response.GetData<List<Donor>>();
        }

        public virtual void UpdateDonor(long id, string name, string address, string phone)
        {
            var req = new Request { Type = RequestType.UPDATE_DONOR };
            var data = new { Name = name, Address = address, Phone = phone };
            req.SetData(data);
            SendRequest(req);
            Response response = ReadResponse();
            if (response.Type == ResponseType.ERROR)
            {
                throw new Exception(response.ErrorMessage);
            }
        }

        private void CloseConnection()
        {
            finished = true;
            try
            {
                input?.Close();
                output?.Close();
                stream?.Close();
                connection?.Close();
                waitHandle?.Close();
                client = null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        private void SendRequest(Request request)
        {
            try
            {
                if (output == null)
                {
                    throw new InvalidOperationException("Connection is not initialized. Call Login first and ensure the server is running.");
                }
                string jsonReq = JsonSerializer.Serialize(request);
                output.WriteLine(jsonReq);
                output.Flush();
            }
            catch (Exception e)
            {
                throw new Exception("Error sending object " + e);
            }
        }

        private Response ReadResponse()
        {
            Response response = null;
            try
            {
                if (waitHandle == null)
                {
                    throw new InvalidOperationException("Connection is not initialized. Call Login first and ensure the server is running.");
                }
                waitHandle.WaitOne();
                lock (responses)
                {
                    response = responses.Dequeue();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            return response;
        }

        private void InitializeConnection()
        {
            try
            {
                connection = new TcpClient(host, port);
                stream = connection.GetStream();
                output = new StreamWriter(stream);
                input = new StreamReader(stream);
                finished = false;
                waitHandle = new AutoResetEvent(false);
                StartReader();
            }
            catch (Exception e)
            {
                CloseConnection();
                throw new InvalidOperationException("Failed to initialize connection to server.", e);
            }
        }

        private void StartReader()
        {
            Thread tw = new Thread(Run);
            tw.Start();
        }

        public virtual void Run()
        {
            while (!finished)
            {
                try
                {
                    string line = input.ReadLine();
                    if (line == null)
                    {
                        finished = true;
                        break;
                    }
                    
                    Response response = JsonSerializer.Deserialize<Response>(line);
                    if (IsUpdate(response))
                    {
                        HandleUpdate(response);
                    }
                    else
                    {
                        lock (responses)
                        {
                            responses.Enqueue(response);
                        }
                        waitHandle.Set();
                    }
                }
                catch (IOException)
                {
                    finished = true;
                    break;
                }
                catch (SocketException)
                {
                    finished = true;
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Reading error " + e);
                }
            }
        }

        private bool IsUpdate(Response response)
        {
            return response.Type == ResponseType.UPDATE || 
                   response.Type == ResponseType.NEW_DONATION || 
                   response.Type == ResponseType.DONOR_UPDATED;
        }

        private void HandleUpdate(Response response)
        {
            if (response.Type == ResponseType.NEW_DONATION)
            {
                CharityCase updatedCase = response.GetData<CharityCase>();
                try
                {
                    client.DonationAdded(updatedCase);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
            else if (response.Type == ResponseType.DONOR_UPDATED)
            {
                Donor updatedDonor = response.GetData<Donor>();
                try
                {
                    client.DonorUpdated(updatedDonor);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
        }
    }
}
