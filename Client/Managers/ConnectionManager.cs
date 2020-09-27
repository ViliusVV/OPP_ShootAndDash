using Common.DTO;
using Microsoft.AspNetCore.SignalR.Client;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Utilities
{
    class ConnectionManager
    {
        public string ServerUrl { get; set; }
        public HubConnection Connection { get; set; }
        public bool Connected { get => Connection.State == HubConnectionState.Connected; }

        public ConnectionManager(string url)
        {
            Connection = new HubConnectionBuilder()
               .WithUrl(new Uri(url))
               .WithAutomaticReconnect()
               .Build();
        }

        public void ConnectToHub()
        {
            Clock clock = new Clock();
            Connection.StartAsync();

            while (Connection.State == HubConnectionState.Connecting)
            {
                float dt = clock.ElapsedTime.AsSeconds();
                if (dt > 0.5)
                {
                    clock.Restart();
                    Console.WriteLine("Connecting...");
                }
            }
            if (Connection.State != HubConnectionState.Connected)
            {
                Console.WriteLine("Connection failed!");
                Environment.Exit(1);
            }
            else
            {
                Console.WriteLine("Connection succesfull!");
            }
            Console.WriteLine(Connection.State);
        }
    }
}
