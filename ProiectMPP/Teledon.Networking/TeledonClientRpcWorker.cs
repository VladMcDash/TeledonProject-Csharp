using System;
using System.IO;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading;
using ProiectMPP.TeledonProject.Domain;
using Teledon.Services;

namespace Teledon.Networking
{
    public class TeledonClientRpcWorker : ITeledonObserver
    {
        private ITeledonServices server;
        private TcpClient connection;
        private NetworkStream stream;
        private StreamReader input;
        private StreamWriter output;
        private volatile bool connected;

        public TeledonClientRpcWorker(ITeledonServices server, TcpClient connection)
        {
            this.server = server;
            this.connection = connection;
            try
            {
                stream = connection.GetStream();
                output = new StreamWriter(stream);
                input = new StreamReader(stream);
                connected = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public virtual void Run()
        {
            while (connected)
            {
                try
                {
                    string requestLine = input.ReadLine();
                    if (requestLine == null) continue;

                    Request request = JsonSerializer.Deserialize<Request>(requestLine);
                    Response response = ProcessRequest(request);

                    if (response != null)
                    {
                        SendResponse(response);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    break;
                }
                
                try
                {
                    Thread.Sleep(100);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
            try
            {
                input.Close();
                output.Close();
                stream.Close();
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error " + e);
            }
        }

        public virtual void DonationAdded(CharityCase updatedCase)
        {
            Response resp = new Response { Type = ResponseType.NEW_DONATION };
            resp.SetData(updatedCase);
            try
            {
                SendResponse(resp);
            }
            catch (Exception e)
            {
                throw new Exception("Sending error: " + e);
            }
        }

        public virtual void DonorUpdated(Donor updatedDonor)
        {
            Response resp = new Response { Type = ResponseType.DONOR_UPDATED };
            resp.SetData(updatedDonor);
            try
            {
                SendResponse(resp);
            }
            catch (Exception e)
            {
                throw new Exception("Sending error: " + e);
            }
        }

        private Response ProcessRequest(Request request)
        {
            Response response = null;
            if (request.Type == RequestType.LOGIN)
            {
                Console.WriteLine("Login request ...");
                Volunteer volunteer = request.GetData<Volunteer>();
                try
                {
                    server.Login(volunteer, this);
                    var resp = new Response { Type = ResponseType.OK };
                    resp.SetData(volunteer);
                    return resp;
                }
                catch (Exception e)
                {
                    connected = false;
                    return new Response { Type = ResponseType.ERROR, ErrorMessage = e.Message };
                }
            }
            if (request.Type == RequestType.LOGOUT)
            {
                Console.WriteLine("Logout request ...");
                Volunteer volunteer = request.GetData<Volunteer>();
                try
                {
                    server.Logout(volunteer, this);
                    connected = false;
                    return new Response { Type = ResponseType.OK };
                }
                catch (Exception e)
                {
                    return new Response { Type = ResponseType.ERROR, ErrorMessage = e.Message };
                }
            }
            if (request.Type == RequestType.GET_CASES)
            {
                Console.WriteLine("Get cases request ...");
                try
                {
                    var cases = server.GetAllCases();
                    response = new Response { Type = ResponseType.OK };
                    response.SetData(cases);
                    return response;
                }
                catch (Exception e)
                {
                    return new Response { Type = ResponseType.ERROR, ErrorMessage = e.Message };
                }
            }
            if (request.Type == RequestType.ADD_DONATION)
            {
                Console.WriteLine("Add donation request ...");
                try
                {
                    var data = JsonSerializer.Deserialize<JsonElement>(request.JsonData);
                    string name = data.GetProperty("Name").GetString();
                    string address = data.GetProperty("Address").GetString();
                    string phone = data.GetProperty("Phone").GetString();
                    long caseId = data.GetProperty("CaseId").GetInt64();
                    double amount = data.GetProperty("Amount").GetDouble();
                    
                    server.AddDonation(name, address, phone, caseId, amount);
                    return new Response { Type = ResponseType.OK };
                }
                catch (Exception e)
                {
                    return new Response { Type = ResponseType.ERROR, ErrorMessage = e.Message };
                }
            }
            if (request.Type == RequestType.SEARCH_DONORS)
            {
                Console.WriteLine("Search donors request ...");
                try
                {
                    string namePart = request.GetData<string>();
                    var donors = server.SearchDonors(namePart);
                    response = new Response { Type = ResponseType.OK };
                    response.SetData(donors);
                    return response;
                }
                catch (Exception e)
                {
                    return new Response { Type = ResponseType.ERROR, ErrorMessage = e.Message };
                }
            }
            if (request.Type == RequestType.UPDATE_DONOR)
            {
                Console.WriteLine("Update donor request ...");
                try
                {
                    var data = JsonSerializer.Deserialize<JsonElement>(request.JsonData);
                    long id = data.GetProperty("Id").GetInt64();
                    string name = data.GetProperty("Name").GetString();
                    string address = data.GetProperty("Address").GetString();
                    string phone = data.GetProperty("Phone").GetString();
                    
                    server.UpdateDonor(id,name, address, phone);
                    return new Response { Type = ResponseType.OK };
                }
                catch (Exception e)
                {
                    return new Response { Type = ResponseType.ERROR, ErrorMessage = e.Message };
                }
            }
            return response;
        }

        private void SendResponse(Response response)
        {
            string jsonResp = JsonSerializer.Serialize(response);
            lock (output)
            {
                output.WriteLine(jsonResp);
                output.Flush();
            }
        }
    }
}
