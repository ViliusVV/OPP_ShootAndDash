using Common.Utilities;
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
			if (temp.CheckText() == "Small")
			{
				GameApplication.GetInstance().AimCursor.ChangeSize(1);
				OurLogger.Log("Small cursor");
			}
			else if (temp.CheckText() == "Medium")
			{
				GameApplication.GetInstance().AimCursor.ChangeSize(2);
				OurLogger.Log("Medium cursor");
			}
			else if (temp.CheckText() == "Big")
			{
				GameApplication.GetInstance().AimCursor.ChangeSize(3);
				OurLogger.Log("Big cursor");
			}
		}
	}
}
