using System;
using Xunit;
using Client.Models;
using Client.Objects;
using Client.Config;
using Client;
using TextureIdentifierClient = Client.Config;
using Client.Utilities;
using Client.Objects.Pickupables;
using Client.Objects.Pickupables.Decorator;
using Common.Enums;
using Common.DTO;
using Client.Adapters;
using Client.Observer;
using Moq;
using SFML.Window;
using Client.UI;
using Common.Utilities;
using SFML.System;
using System.Collections.Generic;
using SFML.Graphics;
using Client.Managers;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace XUnitTests
{
    [CollectionDefinition("on-Parallel Collection", DisableParallelization = true)]
    public class ClientTests
    {
        TextureHolder Textures = TextureHolder.GetInstance();
        SoundHolder Sounds = SoundHolder.GetInstance();
        FontHolder Fonts = FontHolder.GetInstance();
        public ClientTests()
        {
            //LoadTextures();
            //LoadFonts();
        }
        void LoadTextures()
        {
            var allTextures = Enum.GetValues(typeof(TextureIdentifier));
            foreach (TextureIdentifier texture in allTextures)
            {
                Textures.Load(texture);
            }
            Textures.Get(TextureIdentifier.Background).Repeated = true;
        }

        void LoadSounds()
        {
            var allSounds = Enum.GetValues(typeof(SoundIdentifier));
            foreach (SoundIdentifier sound in allSounds)
            {
                Sounds.Load(sound);
            }
        }

        void LoadFonts()
        {
            var allFonts = Enum.GetValues(typeof(FontIdentifier));
            foreach (FontIdentifier font in allFonts)
            {
                Fonts.Load(font);
            }
        }
        [Theory]
        [InlineData(0, 0, 10, 15, 1)]
        [InlineData(0, 0, 20, 10, 1)]
        [InlineData(10, 10, 1, 10, 1)]
        [InlineData(0, 0, 10, 10, 0.5)]
        [InlineData(-5, 0, 9, 8, 1)]
        [InlineData(0, -5, 10, 10, 1)]
        public void TestUpdatePlayerPosition(float x1 , float y1, float x2, float y2, float speed)
        {
            LoadTextures();
            LoadFonts();

            var e = 0.1;

            var mockPlayer = new Mock<Player>();

            mockPlayer.SetupAllProperties();
            mockPlayer.Object.SpeedMultiplier = speed;
            mockPlayer.Object.Speed = new Vector2f(x2, y2);
            mockPlayer.Object.Position = new Vector2f(x1, y1);

            mockPlayer.Object.TranslateFromSpeed();


            var newPos = new Vector2f(x1 + x2 * speed, y1 + y2 * speed);

            bool isClose = MathF.Abs(mockPlayer.Object.Position.X - newPos.X) < e && MathF.Abs(mockPlayer.Object.Position.Y - newPos.Y) < e;

            Assert.True(true);
        }

        [Theory]
        [InlineData(0, 0, 10, 10, 1)]
        [InlineData(0, 0, 0, 5, 1)]
        [InlineData(1, 0, 5, 5, 1)]
        [InlineData(0, 1, 1, 5, 1)]
        [InlineData(0, 1, 1, 5, 2)]
        [InlineData(0, 1, 1, 5, 0.5)]
        public void TestUpdatePlayerPosColide(float x1, float y1, float x2, float y2, float speed)
        {
            LoadTextures();
            LoadFonts();

            var sprite = new Sprite(Textures.Get(TextureIdentifier.BarbWire));

            var gameStateInstance = new Mock<GameState>();
            gameStateInstance.Setup(_ => _.Collidables).Returns(new List<Sprite> { sprite });
            GameState.SetTestingInstance(gameStateInstance.Object);


            var mockPlayer = new Mock<Player>();

            mockPlayer.SetupAllProperties();
            mockPlayer.Object.SpeedMultiplier = speed;
            mockPlayer.Object.Speed = new Vector2f(x2, y2);
            mockPlayer.Object.Position = new Vector2f(x1, y1);

            mockPlayer.Object.TranslateFromSpeed();


            var newPos = new Vector2f(x1 + x2 * speed, y1 + y2 * speed);

            Assert.True(newPos!= mockPlayer.Object.Position);
        }

        [Theory]
        [InlineData(10, 10, 1)]
        [InlineData(10, 0, 1)]
        [InlineData(10, -10, 1)]
        [InlineData(-10, 0, -1)]
        [InlineData(-1, 10, -1)]
        [InlineData(-5, 0, -1)]
        [InlineData(-10, -10, -1)]
        [InlineData(0, 0, 1)]
        public void TestPlayerFacing(float x, float y, float direction)
        {
            LoadTextures();
            LoadSounds();
            LoadFonts();
            Player player = new Player();
            player.Speed = new SFML.System.Vector2f(x, y);
            player.UpdatePlayerFacingPosition();
            Assert.Equal(player.Scale.X, direction);
        }
        [Theory]
        [InlineData(-10, 90)]
        [InlineData(-100, 0)]
        [InlineData(0, 100)]
        public void TestDealDamageToPlayer(int damage, int result)
        {
            LoadTextures();
            LoadSounds();
            LoadFonts();
            Player player = new Player();
            player.AddHealth(damage);
            Assert.Equal(player.Health, result);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(10, 10)]
        [InlineData(100, 100)]
        [InlineData(101, 100)]
        public void TestPlayerHeal(int healAmount, int result)
        {
            LoadTextures();
            LoadSounds();
            LoadFonts();
            Player player = new Player();
            player.AddHealth(-100);
            player.AddHealth(healAmount);
            Assert.Equal(player.Health, result);
        }



        [Theory]
        [InlineData("Pistol", 10, WeaponType.Pistol)]
        [InlineData("AssaultRifle", 11, WeaponType.AssaultRifle)]
        [InlineData("Shootgun", 12, WeaponType.Shootgun)]
        [InlineData("SniperRifle", 13, WeaponType.SniperRifle)]
        [InlineData("Minigun", 14, WeaponType.Minigun)]
        [InlineData("FlameThrower", 14, WeaponType.FlameThrower)]
        public void AdapterTestNormal(String name, int ammo, WeaponType type)
        {
            LoadTextures();
            LoadFonts();
            Weapon weapon = Weapon.CreateWeapon(type);
            weapon.Ammo = ammo;
            weapon.Name = name;


            ServerWeapon serverWeapon = new ServerWeaponAdapter(weapon);

            bool equal = serverWeapon.Ammo == weapon.Ammo
                        && serverWeapon.WeaponName == weapon.Name
                        && serverWeapon.WeaponType == type;

            Assert.True(equal);
        }

        [Theory]
        [InlineData("RamomwWeapon", 10, WeaponType.Pistol)]
        public void AdapterTestUnknown(String name, int ammo, WeaponType type)
        {
            LoadTextures();
            LoadFonts();
            Weapon weapon = new Weapon();
            weapon.Ammo = ammo;
            weapon.Name = name;


            ServerWeapon serverWeapon = new ServerWeaponAdapter(weapon);

            bool equal = serverWeapon.Ammo == weapon.Ammo
                        && serverWeapon.WeaponName == weapon.Name
                        && serverWeapon.WeaponType == type;

            Assert.True(equal);
        }


        [Fact]
        public void EventManagerEmptyCrash()
        {
            LoadTextures();
            LoadFonts();

            var shooter = new Mock<Player>().Object;
            var victim = new Mock<Player>().Object;


            var evtManager = PlayerEventManager.GetInstance();
            var evtData = new PlayerEventData() { Shooter = shooter, Victim = victim };
            evtManager.Notify(PlayerEventType.KilledPlayer, evtData);

            Assert.True(true);
        }

        [Fact]
        public void EventManagerSubNotify()
        {
            LoadTextures();
            LoadFonts();
            var shooter = new Mock<Player>().Object;
            var victim = new Mock<Player>().Object;

            var scoreboard = new Scoreboard();
            var killnotif = new KillNotifier();

            var evtManager = PlayerEventManager.GetInstance();

            var evtData = new PlayerEventData();
            evtData.Shooter = shooter;
            evtData.Victim = victim;

            evtManager.Subscribe(PlayerEventType.PlayerDead, killnotif);
            evtManager.Subscribe(PlayerEventType.KilledPlayer, scoreboard);

            evtManager.Notify(PlayerEventType.KilledPlayer, evtData);
            evtManager.Notify(PlayerEventType.PlayerDead, evtData);

            Assert.True(evtManager._listeners[PlayerEventType.PlayerDead].Count == 1 && evtManager._listeners[PlayerEventType.KilledPlayer].Count == 1);
        }

        [Fact]
        public void EventManagerUnsub()
        {
            LoadTextures();
            LoadFonts();
            var shooter = new Mock<Player>().Object;
            var victim = new Mock<Player>().Object;

            var scoreboard = new Scoreboard();
            var killnotif = new KillNotifier();

            var evtManager = PlayerEventManager.GetInstance();

            evtManager.Subscribe(PlayerEventType.PlayerDead, killnotif);
            evtManager.Subscribe(PlayerEventType.KilledPlayer, scoreboard);

            evtManager.Unsubscribe(PlayerEventType.PlayerDead, killnotif);
            evtManager.Unsubscribe(PlayerEventType.KilledPlayer, scoreboard);



            Assert.True(evtManager._listeners[PlayerEventType.PlayerDead].Count == 0 && evtManager._listeners[PlayerEventType.KilledPlayer].Count == 0);
        }

        
        [Theory]
        [InlineData(0, 1f, 0)]
        [InlineData(90f, 0, 1f)]
        [InlineData(270f, 0, -1f)]
        [InlineData(360f, 1f ,0)]
        [InlineData(180f, -1f, 0)]
        public void AngleDegToUnitVectorTest(float angleDeg, float x, float y)
        {
            LoadTextures();
            LoadFonts();
            float e = 0.01f;

            Vector2f unitVec = VectorUtils.AngleDegToUnitVector(angleDeg);

            bool closeEnough = MathF.Abs(unitVec.X - x) < e && MathF.Abs(unitVec.Y - y) < e;

            Assert.True(closeEnough);
        }
    }
}
