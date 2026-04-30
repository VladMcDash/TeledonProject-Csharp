using Grpc.Core;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using ProiectMPP.TeledonProject.Domain;
using Teledon.Grpc; // Namespace-ul generat din fisierul proto
using Teledon.Services;

namespace Teledon.Server
{
    public class TeledonGrpcService : TeledonService.TeledonServiceBase, ITeledonObserver
    {
        private static readonly ConcurrentDictionary<string, Volunteer> LoggedInByUsername = new();
        private readonly ITeledonServices _server;
        
        private IServerStreamWriter<Notification> _responseStream;//
        private readonly object _streamLock = new();

        public TeledonGrpcService(ITeledonServices server)
        {
            _server = server;
        }

        // --- 1. LOGIN (Aici folosim Server Streaming pentru notificări) ---
        public override async Task Login(LoginRequest request, IServerStreamWriter<Notification> responseStream, ServerCallContext context)
        {
            _responseStream = responseStream;
            
            var volunteer = new Volunteer(request.Username, request.Password, "");
            
            try
            {
                _server.Login(volunteer, this);
                LoggedInByUsername[volunteer.Username] = volunteer;

                while (!context.CancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(1000, context.CancellationToken); 
                }
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated, ex.Message));
            }
            finally
            {
                try
                {
                    _server.Logout(volunteer, this);
                }
                catch
                {
                    // Best-effort logout to avoid breaking the stream cleanup.
                }
                LoggedInByUsername.TryRemove(volunteer.Username, out _);
            }
        }
        
        // --- 2. LOGOUT ---
        public override Task<Empty> Logout(UserRequest request, ServerCallContext context)
        {
            if (LoggedInByUsername.TryRemove(request.Username, out var volunteer))
            {
                _server.Logout(volunteer, this);
            }
            return Task.FromResult(new Empty());
        }

        // --- 3. GET ALL CASES ---
        public override Task<CaseListResponse> GetAllCases(Empty request, ServerCallContext context)
        {
            var cases = _server.GetAllCases();
            var response = new CaseListResponse();
            
            // Mapăm cazurile din entitățile C# în mesajele definite în .proto
            foreach (var c in cases)
            {
                response.Cases.Add(new ProtoCharityCase 
                { 
                    Id = c.Id, 
                    Name = c.Name, 
                    TotalDonated = c.TotalAmount 
                });
            }
            return Task.FromResult(response);
        }

        // --- 4. ADD DONATION ---
        public override Task<Empty> AddDonation(DonationRequest request, ServerCallContext context)
        {
            try 
            {
                _server.AddDonation(
                    request.Donor.Name, 
                    request.Donor.Address, 
                    request.Donor.PhoneNumber, 
                    request.CharityCase.Id, 
                    request.Amount
                ); 
                return Task.FromResult(new Empty());
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, ex.Message));
            }
        }

        // --- 5. SEARCH DONORS ---
        public override Task<DonorListResponse> SearchDonors(SearchDonorRequest request, ServerCallContext context)
        {
            var donors = _server.SearchDonors(request.Keyword);
            var response = new DonorListResponse();
            foreach (var d in donors)
            {
                response.Donors.Add(new ProtoDonor
                {
                    Id = d.Id,
                    Name = d.Name, // ACUM FOLOSIM DOAR NAME
                    Address = d.Address,
                    PhoneNumber = d.PhoneNumber
                });
            }
            return Task.FromResult(response);
        }

        // --- 6. UPDATE DONOR ---
        public override Task<Empty> UpdateDonor(DonorRequest request, ServerCallContext context)
        {
            try
            {
                var pd = request.Donor;
        
                _server.UpdateDonor(pd.Id, pd.Name, pd.Address, pd.PhoneNumber);
        
                return Task.FromResult(new Empty());
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
        }

        private static void SplitName(string fullName, out string firstName, out string lastName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                firstName = "";
                lastName = "";
                return;
            }

            var parts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1)
            {
                firstName = parts[0];
                lastName = "";
                return;
            }

            firstName = parts[0];
            lastName = string.Join(" ", parts, 1, parts.Length - 1);
        }

        private static string CombineName(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName)) return lastName?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(lastName)) return firstName.Trim();
            return (firstName + " " + lastName).Trim();
        }


        public void DonationAdded(CharityCase updatedCase)
        {
            if (_responseStream == null) return;

            var notification = new Notification
            {
                Type = Notification.Types.Type.DonationAdded,
                Message = $"{updatedCase.Id}|{updatedCase.TotalAmount}"
            };

            lock (_streamLock)
            {
                try
                {
                    _responseStream.WriteAsync(notification).GetAwaiter().GetResult();
                }
                catch
                {
                    // Probabil clientul s-a deconectat
                }
            }
        }
        

        public void DonorUpdated(Donor updatedDonor)
        {
            if (_responseStream == null) return;

            var notification = new Notification
            {
                Type = Notification.Types.Type.DonorUpdated,
                Message = updatedDonor.Id.ToString()
            };

            lock (_streamLock)
            {
                try
                {
                    _responseStream.WriteAsync(notification).Wait();
                }
                catch
                {
                    // Ignore push failures when the client is gone.
                }
            }
        }
    }
}