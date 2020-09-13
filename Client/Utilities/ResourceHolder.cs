using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using Client.Config;
using SFML.Audio;
using SFML.Graphics;

namespace Client
{
    // Helper class to hold all loadable resources
    abstract class ResourceHolder<TIdentifier, TResource>
    {
        // Hold all textures in map
        private readonly Dictionary<TIdentifier, TResource> resourceMap = new Dictionary<TIdentifier, TResource>();

        public abstract void Load(TIdentifier id);

        public TResource Get(TIdentifier id)
        {
            return resourceMap[id];
        }

        protected void InsertResource(TIdentifier id, TResource resource)
        {
            resourceMap.Add(id, resource);
        }
    }

    // Loads and stores all game textures
    class TextureHolder : ResourceHolder<TextureIdentifier, Texture>
    {
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

    // Loads and stores all game's custom fonts
    class FontHolder : ResourceHolder<FontIdentifier, Font>
    {
        public override void Load(FontIdentifier id)
        {
            var font = new Font(id.GetResoucePath());

            InsertResource(id, font);
        }
    }


    // Loads and stores all game's music and sound effects
    class SoundHolder : ResourceHolder<SoundIdentifier, Sound>
    {

        public override void Load(SoundIdentifier id)
        {
            // All music and sounds must be using same channel/bitrate/etc. configuration 
            // to prevent bad stuff from happening
            var soundBuffer = new SoundBuffer(id.GetResoucePath());
            var sound = new Sound(soundBuffer);

            InsertResource(id, sound);
        }
    }
}
