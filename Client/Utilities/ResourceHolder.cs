using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using Client.Config;
using Common.Utilities;
using SFML.Audio;
using SFML.Graphics;

namespace Client.Utilities
{
    // Helper class to hold all loadable resources
    public abstract class ResourceHolder<TIdentifier, TResource>
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
            //OurLogger.Log("Loaded resource: " + GameResourceHelper.GetResoucePath<Enum>(id as Enum));
            //GameApplication.defaultLogger.LogMessage(10, "Loaded resource: " + GameResourceHelper.GetResoucePath<Enum>(id as Enum));
            if(!resourceMap.ContainsKey(id))
            {
                resourceMap.Add(id, resource);
            }
        }
    }
}
