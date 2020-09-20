using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Hubs
{
    public class ShotAndDashHub: Hub
    {
        public async Task ReceivePos(string clientId, string position)
        {
            Console.WriteLine($"ClientId {clientId} with pos {position}");
            await Clients.All.SendAsync("ReceivePos", $"Player {clientId} with {position}");
        }

        public async override Task OnConnectedAsync()
        {
            Console.WriteLine($"Client connected: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }


    }
}
