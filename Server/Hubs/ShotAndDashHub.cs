using Common;
using Common.DTO;
using Microsoft.AspNetCore.SignalR;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Hubs
{
    public class ShotAndDashHub: Hub
    {
        public readonly GameManager _gameManager = GameManager.GetInstance();

        public async Task ReceivePos(PlayerDTO dto)
        {wadas
            _gameManager.Players[Context.ConnectionId] = dto;
            await Clients.All.SendAsync("UpdateState", _gameManager.GetGameStateDTO());
        }

        public async Task SpawnPlayer(PlayerDTO playerDTO)
        {
            Console.WriteLine("Player {0} connected", playerDTO);
            _gameManager.Players.Add(Context.ConnectionId, playerDTO);
            //await Clients.AllExcept(Context.ConnectionId).SendAsync("CreatePlayer", playerDTO);
            await Clients.All.SendAsync("CreatePlayer", _gameManager.GetGameStateDTO());
        }

        public async override Task OnConnectedAsync()
        {
            Console.WriteLine($"Player trying to conect with connection ID {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        public async Task OnDisconnectedAsync()
        {
            Console.WriteLine($"Player disconnected with ID {Context.ConnectionId}");
            _gameManager.Players.Remove(Context.ConnectionId);
            await base.OnDisconnectedAsync( new Exception("Disconnected player"));
        }
    }
}
