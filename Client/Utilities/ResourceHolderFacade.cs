using Client.Config;
using Common.Utilities;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Utilities
{
    public class ResourceHolderFacade
    {
        private static readonly ResourceHolderFacade _instance = new ResourceHolderFacade();
        public TextureHolder Textures { get; private set; } = TextureHolder.GetInstance();
        public SoundHolder Sounds { get; private set; } = SoundHolder.GetInstance();
        public FontHolder Fonts { get; private set; } = FontHolder.GetInstance();
        public SoundVolume CurrentVolume { get; private set; } = SoundVolume.GetInstance();

        private ResourceHolderFacade()
        {
            LoadTextures();
            LoadSounds();
            LoadFonts();
        }

        public static ResourceHolderFacade GetInstance()
        {
            return _instance;
        }
        // Load all game textures
        void LoadTextures()
        {
            OurLogger.Log("Loading textures...");

            // Iterate over all textures and load
            var allTextures = Enum.GetValues(typeof(TextureIdentifier));
            foreach (TextureIdentifier texture in allTextures)
            {
                Textures.Load(texture);
            }

            // Set special properties for some textures
            Textures.Get(TextureIdentifier.Background).Repeated = true;
        }


        // Load all music and sound efects
        void LoadSounds()
        {
            OurLogger.Log("Loading sounds...");

            var allSounds = Enum.GetValues(typeof(SoundIdentifier));
            foreach (SoundIdentifier sound in allSounds)
            {
                Sounds.Load(sound);
            }
        }


        // Load all custom fonts
        void LoadFonts()
        {
            OurLogger.Log("Loading fonts...");

            var allFonts = Enum.GetValues(typeof(FontIdentifier));
            foreach (FontIdentifier font in allFonts)
            {
                Fonts.Load(font);
            }
        }
    }
}
