using Client.Managers.Iterator.Repositories;
using Client.Managers.Proxy;
using Client.Models;
using Client.Objects;
using Client.Objects.Destructables;
using Client.Objects.Indestructables;
using Client.Objects.Memento;
using Client.Utilities;
using Common;
using Common.DTO;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Managers
{
    public class GameState
    {
        private static GameState _instance = new GameState();

        public Random Random { get; private set; }

        public List<Player> Players { get; set; }
        //public List<Pickupable> Pickupables { get; set; }
        public virtual List<Sprite> Collidables { get; set; }
        public PickupableRepository PickupableRep { get; set; }
        public PlayerRepository PlayerRep { get; set; }
        public TileMap TileMap { get; set; }
        //public List<Sprite> NonCollidables { get; set; }
        public NonCollidableRepository NonCollidableRep { get; set; }

        public bool PortalObjectCreated { get; set; }
        //Composite controls
        public bool ControlsCheck { get; set; }

        public bool CloseWindow { get; set; } 
        //-----------------------
        //public ConnectionManager ConnectionManager { get; set; }
        public ConnectionManagerProxy ConnectionManagerProxy { get; set; }

        public GameState()
        {
            this.Players = new List<Player>();
            this.PlayerRep = new PlayerRepository();
            //this.Pickupables = new List<Pickupable>();
            this.PickupableRep = new PickupableRepository();
            this.Collidables = new List<Sprite>();
            //this.NonCollidables = new List<Sprite>();
            this.NonCollidableRep = new NonCollidableRepository();
            this.Random = new Random();
            PortalObjectCreated = false;
            ControlsCheck = true;
            CloseWindow = false;
        }

        public static GameState GetInstance()
        {
            return _instance;
        }

        public static void SetTestingInstance(GameState instance)
        {
            _instance = instance;
        }

        public void InitRandom(int seed)
        {
            this.Random = new Random(seed);
        }


        public void ToDTO()
        {
            ServerGameState dto = new ServerGameState();
            foreach(Player player in Players)
            {
                dto.Players.Add(player.ToDTO());
            }
        }
        
        public void FromDTO(ServerGameState dto)
        {
            foreach(ServerPlayer playerDto in dto.Players)
            {
                Player player = Players.Find(p => p.Name.Equals(playerDto.Name));

                if (player != null && !player.IsMainPlayer)
                {
                    player.RefreshData(playerDto);
                }
            }
        }
    }
}
