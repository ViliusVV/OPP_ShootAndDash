using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Common.Utilities;

namespace Client.UI
{
	class ButtonsClass
	{
		public CompositeUI composite;
		public CompositeUI controlComposite;
		public bool Show = false;
		
		public Clock ChangeTimer { get; set; } = new Clock();

		public ButtonsClass()
		{
			//main menu
			composite = new CompositeUI();
			controlComposite = new CompositeUI();

			var playButton = new Button();
			playButton.Position = new Vector2f(200, 200);
			playButton.SetText("Play");

			var settingsButton = new Button();
			settingsButton.Position = new Vector2f(200, 250);
			settingsButton.SetText("Settings");
			settingsButton.SetToggle(true);

			var exitButton = new Button();
			exitButton.Position = new Vector2f(200, 300);
			exitButton.SetText("Exit");

			composite.Add(playButton);
			composite.Add(settingsButton);

			//settings menu
			CompositeUI leaf = new CompositeUI();

			var rightControlButton = new Button();
			rightControlButton.Position = new Vector2f(300, 250);
			rightControlButton.SetText("WASD");

			var arrowControlButton = new Button();
			arrowControlButton.Position = new Vector2f(400, 250);
			arrowControlButton.SetText("Arrows");

			var backButton = new Button();
			backButton.Position = new Vector2f(350, 300);
			backButton.SetText("Back");

			leaf.Add(rightControlButton);
			leaf.Add(arrowControlButton);
			leaf.Add(backButton);

			composite.Add(leaf);

			controlComposite.Add(playButton);
			controlComposite.Add(settingsButton);
			controlComposite.Add(leaf);
			controlComposite.Add(exitButton);

			//---------------------
			//leaf.CurrentChosen = true;
			composite.Add(exitButton);
			composite.CurrentChosen = true;
			//CompositeUI unlimited = (CompositeUI)composite.children[2];
			//composite.children = unlimited.children;

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

		public void chooseComposite()
		{
			if (composite.HasSelection())
			{
				composite.children[composite.selectedChild].Activate();
				Button depression = (Button)composite.children[composite.selectedChild];
				if (depression.CheckToggle())
				{
					CompositeUI newComposite = (CompositeUI)composite.children[composite.selectedChild + 1];
					//controlComposite = composite;
					composite.children = newComposite.children;
				}
			}
		}

		public void returnComposite()
		{
			Button temp = (Button)composite.children[composite.selectedChild];
			
			if(temp.CheckText() == "Back")
			{
				OurLogger.Log(temp.CheckText());
				composite = controlComposite;
				composite.CurrentChosen = true;
			}
		}
		
	}
}
