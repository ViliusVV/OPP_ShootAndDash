using Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Client.Observer
{
    public class PlayerEventManager
    {
        private static readonly PlayerEventManager _instance = new PlayerEventManager();

        public Dictionary<PlayerEventType, List<IPlayerEventListener>> _listeners = new Dictionary<PlayerEventType, List<IPlayerEventListener>>();

        private Object lockObj = new Object();

        private PlayerEventManager()
        {
            foreach(PlayerEventType type in Enum.GetValues(typeof(PlayerEventType)))
            {
                _listeners.Add(type, new List<IPlayerEventListener>());
            }
        }

        public static PlayerEventManager GetInstance()
        {
            return _instance;
        }



        public void Subscribe(PlayerEventType type, IPlayerEventListener listener)
        {
            //OurLogger.Log($"{listener} subscribed to event type {type}");
            GameApplication.defaultLogger.LogMessage(20, $"{listener} subscribed to event type {type}");

            _listeners[type].Add(listener);
        }

        public void Unsubscribe(PlayerEventType type, IPlayerEventListener listener)
        {
            //OurLogger.Log($"{listener} unsubscribed from event type {type}");
            GameApplication.defaultLogger.LogMessage(20, $"{listener} unsubscribed to event type {type}");
            _listeners[type].Remove(listener);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Notify(PlayerEventType type, PlayerEventData eventData)
        {
            foreach (var listener in _listeners[type])
            {
                listener.Update(eventData);
            }
        }
    }
}
