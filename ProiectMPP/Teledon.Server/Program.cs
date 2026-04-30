using System.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProiectMPP.TeledonProject.Repository;
using Teledon.Server;
using Teledon.Services;
using TeledonProject.Repository;
using ConfigurationManager = System.Configuration.ConfigurationManager;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5000, o => o.Protocols = HttpProtocols.Http2);
});

builder.Services.AddGrpc();

string connectionString = ConfigurationManager.ConnectionStrings["teledonDB"].ConnectionString;
Console.WriteLine($"[Server] String de conexiune incarcat: {connectionString}");

var dbUtils = new DbUtils(connectionString);
IVolunteerRepository volunteerRepo = new VolunteerDbRepository(dbUtils);
ICharityCaseRepository caseRepo = new CharityCaseDbRepository(dbUtils);
IDonorRepository donorRepo = new DonorDbRepository(dbUtils);
IDonationRepository donationRepo = new DonationDbRepository(dbUtils);

ITeledonServices serviceImpl = new TeledonServicesImpl(volunteerRepo, caseRepo, donorRepo, donationRepo);

builder.Services.AddSingleton<ITeledonServices>(serviceImpl);

var app = builder.Build();

app.MapGrpcService<TeledonGrpcService>();

app.MapGet("/", () => "Serverul gRPC Teledon ruleaza pe portul 5000.");

Console.WriteLine("[Server] Serverul gRPC porneste pe http://localhost:5000...");
app.Run();