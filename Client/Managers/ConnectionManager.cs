using Common.DTO;
using Common.Utilities;
using Microsoft.AspNetCore.SignalR.Client;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Utilities
{
    public class ConnectionManager
    {
        public string ServerUrl { get; set; }
        public bool Connected { get => Connection.State == HubConnectionState.Connected; }

        public HubConnection Connection { get; set; }
        public Clock ActivityClock { get; set; } = new Clock();

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
                    OurLogger.Log("Connecting...");
                }
            }
            if (Connection.State != HubConnectionState.Connected)
            {
                OurLogger.Log("Connection failed!");
                Environment.Exit(1);
            }
            else
            {
                OurLogger.Log("Connection succesfull!");
            }
            OurLogger.Log(Connection.State.ToString());
        }
    }
}
