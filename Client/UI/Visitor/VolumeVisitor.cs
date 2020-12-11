using System;
using System.Collections.Generic;
using System.Text;
using Client.Utilities;

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
			}
			else if(temp.CheckText() == ">") 
			{
				SoundVolume.GetInstance().ChangeVolume(5);
			}
		}
	}
}
