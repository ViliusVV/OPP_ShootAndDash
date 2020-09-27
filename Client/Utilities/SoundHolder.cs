using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using Client.Config;
using SFML.Audio;
using SFML.Graphics;


namespace Client.Utilities
{
    // Loads and stores all game's music and sound effects
    public class SoundHolder : ResourceHolder<SoundIdentifier, Sound>
    {
        private static readonly SoundHolder _instance = new SoundHolder();

        private SoundHolder() { }

        public static SoundHolder GetInstance()
        {
            return _instance;
        }

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
