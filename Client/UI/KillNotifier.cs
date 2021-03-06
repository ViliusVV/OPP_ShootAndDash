﻿using Client.Observer;
using Common.Utilities;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Client.UI
{
    public class KillNotifier : IPlayerEventListener, Drawable
    {
        public CustomText message = new CustomText(4 * 7);
        private readonly int messageTimeout = 2;
        private Clock timemoutTimer = new Clock();

        public void Draw(RenderTarget target, RenderStates states)
        {
            try
            {
                if (timemoutTimer.ElapsedTime.AsSeconds() < messageTimeout)
                    target.Draw(message);
            }
            catch { }
        }

        public void Update(PlayerEventData eventData)
        {
            lock (GameApplication.GetInstance().SFMLLock)
            {
                OurLogger.Log("Kill notifier notified");
                GameApplication.defaultLogger.LogMessage(11, "Kill notifier notified");

                message.DisplayedString = $"Player {eventData.Shooter.Name} killed {eventData.Victim.Name}";

                var viewPort = GameApplication.GetInstance().GameWindow.GetViewport(GameApplication.GetInstance().MainView);
                var newOrgin = new Vector2f(message.GetLocalBounds().Width / 2f, message.GetLocalBounds().Height / 2f);
                message.Origin = newOrgin;
                message.Position = new Vector2f(viewPort.Width / 2f, viewPort.Height / 1.05f);

                timemoutTimer.Restart();
            }
        }
    }
}
