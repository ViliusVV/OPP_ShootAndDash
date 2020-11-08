using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Observer
{
    interface IPlayerEventListener
    {
        public void Update(PlayerEventData eventData);
    }
}
