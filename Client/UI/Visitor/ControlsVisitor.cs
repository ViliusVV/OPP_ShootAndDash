using Client.Managers;
using Common.Utilities;
using System;
using System.Collections.Generic;
using System.Text;


namespace Client.UI.Visitor
{
	class ControlsVisitor : IVisitor
	{
		public void Visit(Component element)
		{
			Button tempButton = (Button)element;
			if(tempButton.CheckText() == "WASD")
			{
				GameState.GetInstance().ControlsCheck = true;
				OurLogger.Log("Player movement WASD");
			}
			else if(tempButton.CheckText() == "Arrows")
			{
				GameState.GetInstance().ControlsCheck = false;
				OurLogger.Log("Player movement Arrows");
			}
		}
	}
}
