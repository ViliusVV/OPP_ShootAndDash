using Client.Objects.Pickupables.Decorator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Objects.Pickupables.Mediator
{
    public interface IMediator
    {
        public abstract void Send(string message, Weapon weapon);
    }
}
