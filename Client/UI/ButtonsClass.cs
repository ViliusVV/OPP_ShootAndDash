using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Client.UI
{
	class ButtonsClass
	{
		public CompositeUI composite;
		public bool Show = false;
		public Clock ChangeTimer { get; set; } = new Clock();

		public ButtonsClass()
		{
			composite = new CompositeUI();

			var playButton = new Button();
			playButton.Position = new Vector2f(200, 200);
			playButton.SetText("Play");

			var settingsButton = new Button();
			settingsButton.Position = new Vector2f(200, 250);
			settingsButton.SetText("Settings");

			var exitButton = new Button();
			exitButton.Position = new Vector2f(200, 300);
			exitButton.SetText("Exit");

			composite.Add(playButton);
			composite.Add(settingsButton);
			composite.Add(exitButton);
		}

		public void ChangeVisibility()
		{
			if (this.ChangeTimer.ElapsedTime.AsMilliseconds() > 200)
			{
				this.ChangeTimer.Restart();
				if (Show == false)
				{
					Show = true;
				}
				else
				{
					Show = false;
				}
			}
		}
		

	}
}
