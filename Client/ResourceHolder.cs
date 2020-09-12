using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;

namespace Client
{
    abstract class ResourceHolder<TIdentifier, TResource, TParameter>
    {
        private Dictionary<TIdentifier, TResource> resourceMap = new Dictionary<TIdentifier, TResource>();

        public abstract void Load(TIdentifier id, string filename);
        public abstract void Load(TIdentifier id, string filename, TParameter secondParameter);

        public TResource Get(TIdentifier id)
        {
            return resourceMap[id];
        }

        protected void InsertResource(TIdentifier id, TResource resource)
        {
            resourceMap.Add(id, resource);
        }
    }

    class TextureHolder : ResourceHolder<TextureID, Texture, IntRect>
    {
        public override void Load(TextureID id, string filename)
        {
            // Create and load resource
            var texture = new Texture(filename);

            // If loading successful, insert resource to map
            InsertResource(id, texture);
        }

        public override void Load(TextureID id, string filename, IntRect secondParameter)
        {
            // Create and load resource
            var texture = new Texture(filename, secondParameter);

            // If loading successful, insert resource to map
            InsertResource(id, texture);
        }

        public void Load(TextureID id, string filename, bool repeat)
        {
            var texture = new Texture(filename) { Repeated = repeat };

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
