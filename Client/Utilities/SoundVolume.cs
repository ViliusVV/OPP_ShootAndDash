using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Utilities
{
    class SoundVolume
    {
        private static SoundVolume _instance = new SoundVolume();
        private static float Volume = 50;

        public static SoundVolume GetInstance()
        {
            return _instance;
        }
        public void SetVolume(float i)
        {
            Volume = i;
        }

        public float GetVolume()
        {
            return Volume;
        }
        
    }
}
