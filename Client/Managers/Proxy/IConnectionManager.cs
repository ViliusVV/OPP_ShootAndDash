using Microsoft.AspNetCore.SignalR.Client;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Managers.Proxy
{
    public interface IConnectionManager
    {
        public HubConnection Connection { get; set; }
        public Clock ActivityClock { get; set; }
        public bool IsConnected();
        public bool ConnectToHub();

    }
    
}
