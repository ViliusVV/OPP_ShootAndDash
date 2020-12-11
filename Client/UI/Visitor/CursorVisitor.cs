using System;
using System.Collections.Generic;
using System.Text;

namespace Client.UI.Visitor
{
	class CursorVisitor : IVisitor
	{
		public void Visit(Component element)
		{
			Button temp = (Button)element;
			GameApplication.GetInstance().AimCursor.ChangeSize(3);
		}
	}
}
