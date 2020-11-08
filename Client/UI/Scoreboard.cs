using Client.Observer;
using Common.Utilities;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.UI
{
    class Scoreboard: IPlayerEventListener, Drawable
    {
        public Scoreboard()
        {
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            
        }

        public void Update(PlayerEventData eventData)
        {
            OurLogger.Log($"Got event {eventData}");
        }

        public void UpdateScoreboard(int playerCount)
        {
            for (int i = 0; i < playerCount; i++)
            {

            }
        }


    }
}
