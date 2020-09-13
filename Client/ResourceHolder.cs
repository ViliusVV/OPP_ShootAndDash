using System;
using System.Collections.Generic;
using System.Text;
using Client.Config;
using SFML.Audio;
using SFML.Graphics;

namespace Client
{
    // Helper class to hold all loadable resources
    abstract class ResourceHolder<TIdentifier, TResource, TParameter>
    {
        // Hold all textures in map
        private Dictionary<TIdentifier, TResource> resourceMap = new Dictionary<TIdentifier, TResource>();

        public abstract void Load(TIdentifier id);
        public abstract void Load(TIdentifier id, TParameter secondParameter);

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
    class TextureHolder : ResourceHolder<TextureIdentifier, Texture, IntRect>
    {
        public override void Load(TextureIdentifier id)
        {
            var texture = new Texture(id.GetResoucePath());

            InsertResource(id, texture);
        }

        public override void Load(TextureIdentifier id, IntRect secondParameter)
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



    //internal class FontHolder : ResourceHolder<Fonts.ID, Font, object>
    //{
    //    public override void Load(Fonts.ID id, string filename)
    //    {
    //        // Create and load resource
    //        var font = new Font(filename);

    //        // If loading successful, insert resource to map
    //        InsertResource(id, font);
    //    }

    //    public override void Load(Fonts.ID id, string filename, object secondParameter)
    //    {
    //        // Create and load resource
    //        var font = new Font(filename);

    //        // If loading successful, insert resource to map
    //        InsertResource(id, font);
    //    }
    //}
}
