using Microsoft.AspNetCore.SignalR;
using Server.Models;
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
        public async Task SpawnPlayer(PlayerDTO playerDTO)
        {
            Console.WriteLine("Spawning player...");
            Console.WriteLine(playerDTO.ToString());
            await Clients.AllExcept(Context.ConnectionId).SendAsync("CreatePlayer", playerDTO);
        }
        public async override Task OnConnectedAsync()
        {
            Console.WriteLine($"Client connected: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }


    }
}
