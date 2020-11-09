using Client.Models;
using Client.Observer;
using Common.Utilities;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Client.UI
{
    public class Scoreboard: IPlayerEventListener, Drawable
    {
        private Dictionary<Player, CustomText> scores = new Dictionary<Player, CustomText>();

        private Object lockObj = new Object();

        public Scoreboard()
        {

        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            try {
                var lastHeight = 2;
                var keyList = new List<Player>(scores.Keys);

                for (int i = 0; i < keyList.Count; i++)
                {
                    var key = keyList[i];
                    var text = scores[key];
                    text.Position = new Vector2f(0f, lastHeight);
                    target.Draw(text);

                    lastHeight += 18;
                }
            }
            catch { }

            
        }

        public void Update(PlayerEventData eventData)
        {
            lock (GameApplication.GetInstance().SFMLLock)
            {
                OurLogger.Log($"Got event {eventData}");
                if (!scores.ContainsKey(eventData.Shooter))
                {
                    scores.Add(eventData.Shooter, new CustomText(2 * 7));
                }

                if (!scores.ContainsKey(eventData.Victim))
                {
                    scores.Add(eventData.Victim, new CustomText(2 * 7));
                }

                try
                {
                    scores[eventData.Victim].DisplayedString = $"{eventData.Victim.Name} {eventData.Victim.Kills}/{eventData.Victim.Deaths}";
                    scores[eventData.Shooter].DisplayedString = $"{eventData.Shooter.Name} {eventData.Shooter.Kills}/{eventData.Shooter.Deaths}";
                }
                catch { }
            }
        }

        
    }

    class Score
    {
        public int Kills { get; set; }
        public int Deaths { get; set; }

        public override string ToString()
        {
            return $"{Kills}/{Deaths}";
        }
    }
}
