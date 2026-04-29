using System;
using System.Configuration;
using System.Windows.Forms;
using Teledon.Networking;
using Teledon.Services;
using TeledonProject.GUI;

namespace Teledon.Client;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        
        string ip = ConfigurationManager.AppSettings["ip"] ?? "127.0.0.1";
        int port = int.Parse(ConfigurationManager.AppSettings["port"] ?? "55555");
        
        ITeledonServices server = new TeledonServerRpcProxy(ip, port);
        
        Application.Run(new LoginForm(server));
    }
}