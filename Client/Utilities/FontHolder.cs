using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using Client.Config;
using SFML.Audio;
using SFML.Graphics;


namespace Client.Utilities
{
    // Loads and stores all game's custom fonts
    public class FontHolder : ResourceHolder<FontIdentifier, Font>
    {
        private static readonly FontHolder _instance = new FontHolder();

        private FontHolder() { }

        public static FontHolder GetInstance()
        {
            return _instance;
        }

        public override void Load(FontIdentifier id)
        {
            var font = new Font(id.GetResoucePath());

            InsertResource(id, font);
        }
    }
}
