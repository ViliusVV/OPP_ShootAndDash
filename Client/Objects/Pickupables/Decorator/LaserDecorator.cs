using Client.Objects.Pickupables.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Objects.Pickupables.Decorator
{
	public abstract class LaserDecorator : WeaponDecorator
	{
		public LaserDecorator(IMediator mediator) : base(mediator)
		{

		}
	}
}
