using Client.Objects.Pickupables.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Objects.Pickupables.Decorator
{
    public abstract class WeaponDecorator : Weapon
    {
        public WeaponDecorator(IMediator mediator) : base(mediator)
        {
        }


    }
}
