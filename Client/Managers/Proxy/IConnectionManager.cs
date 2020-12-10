using Microsoft.AspNetCore.SignalR.Client;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Managers.Proxy
{
    public interface IConnectionManager
    {
        public bool Connected { get => Connection.State == HubConnectionState.Connected; }
        public HubConnection Connection { get; set; }
        public Clock ActivityClock { get; set; }
        public bool ConnectToHub();


    }
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
                    //OurLogger.Log("Connecting...");
                    GameApplication.defaultLogger.LogMessage(20, "Connecting...");
                }
            }
            if (Connection.State != HubConnectionState.Connected)
            {
                //OurLogger.Log("Connection failed!");
                GameApplication.defaultLogger.LogMessage(50, "Connection failed!");

                Environment.Exit(1);
            }
            else
            {
                //OurLogger.Log("Connection succesfull!");
                GameApplication.defaultLogger.LogMessage(20, "Connection successful!");

            }
            //OurLogger.Log(Connection.State.ToString());
            GameApplication.defaultLogger.LogMessage(10, Connection.State.ToString());

        }
    }
}
