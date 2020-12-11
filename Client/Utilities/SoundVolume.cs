using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Utilities
{
    public class SoundVolume
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

        public void ChangeVolume(float i)
		{
            Volume = Volume + i;
            if (Volume > 100)
            {
                Volume = 100;
            }
            if (Volume < 100)
            {
                Volume = 0;
            }
        }

        public float GetVolume()
        {
            return Volume;
        }
        
    }
}
