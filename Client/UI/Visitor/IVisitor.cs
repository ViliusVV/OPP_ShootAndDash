using System;
using System.Collections.Generic;
using System.Text;

namespace Client.UI.Visitor
{
	interface IVisitor
	{
		void Visit(Component element);
	}
}
