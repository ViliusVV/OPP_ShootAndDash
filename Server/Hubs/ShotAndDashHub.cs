
using Common;
using Common.DTO;
using Common.Utilities;
using Microsoft.AspNetCore.Mvc.Diagnostics;
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


        // Hub server callbacks

        public void LoginPlayerServerEvent(ServerPlayer playerDTO)
        {
            Console.WriteLine("Player {0} connected", playerDTO);
            _gameManager.Players.Add(Context.ConnectionId, playerDTO);
        }

        public async Task UpdateGameStateServer(ServerPlayer dto)
        {
            _gameManager.Players[Context.ConnectionId] = dto;
            await Clients.All.SendAsync("UpdateGameStateClient", _gameManager.GetGameStateDTO());
        }

        public async Task UpdateScoresServer(ServerPlayer shooterOrg, ServerPlayer victim)
        {
            var shooter = _gameManager.Players.Where(p => p.Value.Name.Equals(shooterOrg.Name)).First().Value;
            var deadMan = _gameManager.Players.Where(p => p.Value.Name.Equals(victim.Name)).First().Value;

            deadMan.Health = victim.Health;

            if (victim.IsDead)
            {
                shooter.Kills += 1;
                deadMan.Deaths += 1;

                OurLogger.Log($"{shooter.Name} killed ---> {deadMan.Name}");
            }

            await Clients.All.SendAsync("UpdateScoresClient", shooter, deadMan);
        }

        public void ShootEventServer(ShootEventData shootData)
        {
            OurLogger.Log(shootData.ToString());
            Clients.AllExcept(Context.ConnectionId).SendAsync("ShootEventClient", shootData);
        }



        // Hub native events

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
