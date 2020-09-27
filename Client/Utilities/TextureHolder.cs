using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using Client.Config;
using SFML.Audio;
using SFML.Graphics;

namespace Client.Utilities
{
    // Loads and stores all game textures
    public class TextureHolder : ResourceHolder<TextureIdentifier, Texture>
    {
        private static readonly TextureHolder _instance = new TextureHolder();

        private TextureHolder() { }

        public static TextureHolder GetInstance()
        {
            return _instance;
        }

        public override void Load(TextureIdentifier id)
        {
            var texture = new Texture(id.GetResoucePath());

            InsertResource(id, texture);
        }

        public void Load(TextureIdentifier id, IntRect secondParameter)
        {
            var texture = new Texture(id.GetResoucePath(), secondParameter);

            InsertResource(id, texture);
        }

        public void Load(TextureIdentifier id, bool repeat)
        {
            var texture = new Texture(id.GetResoucePath()) { Repeated = repeat };

            InsertResource(id, texture);
        }
    }
}
