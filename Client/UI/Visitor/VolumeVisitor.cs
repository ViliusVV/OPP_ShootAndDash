using System;
using System.Collections.Generic;
using System.Text;
using Client.Utilities;
using Common.Utilities;

namespace Client.UI.Visitor
{
	class VolumeVisitor : IVisitor
	{
		public void Visit(Component element)
		{
			Button temp = (Button)element;
			if (temp.CheckText() == "<")
			{
				SoundVolume.GetInstance().ChangeVolume(-5);
				OurLogger.Log("Lowering volume");
			}
			else if(temp.CheckText() == ">") 
			{
				SoundVolume.GetInstance().ChangeVolume(5);
				OurLogger.Log("Increasing volume");
			}
		}
	}
}
