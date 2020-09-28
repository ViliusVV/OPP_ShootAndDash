using System;
using Xunit;
using Client.Models;
using Client.Objects;
using Client.Config;
using Client;
using TextureIdentifierClient = Client.Config;

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
        public void Test1()
        {
            Player player = new Player();
            Medkit medkit = new Medkit();
            player.ApplyDamage(-80);
            medkit.Pickup(player);
            Assert.Equal(70, player.Health);
        }
    }
}
