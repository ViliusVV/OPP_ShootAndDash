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

namespace XUnitTests
{
    public class ClientTests
    {
        TextureHolder Textures = TextureHolder.GetInstance();
        SoundHolder Sounds = SoundHolder.GetInstance();
        FontHolder Fonts = FontHolder.GetInstance();
        public ClientTests()
        {
            LoadTextures();
            LoadSounds();
            LoadFonts();
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
        [Fact]
        public void TestUpdatePlayerPosition()
        {
            Assert.False(true, "test is not implemented");
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
            Player player = new Player();
            player.AddHealth(-100);
            player.AddHealth(healAmount);
            Assert.Equal(player.Health, result);
        }

    }
}
