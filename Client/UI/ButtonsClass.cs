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
		public bool Show = false;
		
		public Clock ChangeTimer { get; set; } = new Clock();
		public ButtonsClass()
		{
			//main menu
			composite = new CompositeUI();

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
			rightControlButton.Position = new Vector2f(320, 250);
			rightControlButton.SetText("WASD");

			var arrowControlButton = new Button();
			arrowControlButton.Position = new Vector2f(420, 250);
			arrowControlButton.SetText("Arrows");

			var volumeLowerButton = new Button();
			volumeLowerButton.Position = new Vector2f(300, 300);
			volumeLowerButton.SetText("<");

			var volumeHigherButton = new Button();
			volumeHigherButton.Position = new Vector2f(440, 300);
			volumeHigherButton.SetText(">");

			var volumeButton = new Button();
			volumeButton.Position = new Vector2f(370, 300);
			volumeButton.SetText("69");

			var backButton = new Button();
			backButton.Position = new Vector2f(370, 350);
			backButton.SetText("Back");

			leaf.Add(rightControlButton);
			leaf.Add(arrowControlButton);
			leaf.Add(volumeLowerButton);
			leaf.Add(volumeButton);
			leaf.Add(volumeHigherButton);
			leaf.Add(backButton);

			composite.Add(leaf);
			
			//---------------------
			composite.Add(exitButton);
			composite.CurrentChosen = true;

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

			if (composite.DepthCheck == false)
			{
				if (this.ChangeTimer.ElapsedTime.AsMilliseconds() > 200)
				{
					this.ChangeTimer.Restart();
					if (composite.HasSelection())
					{
						composite.children[composite.selectedChild].Activate();
						Button depression = (Button)composite.children[composite.selectedChild];
						if (depression.CheckToggle())
						{
							composite.DepthCheck = true;
							CompositeUI tempList = (CompositeUI)composite.children[composite.selectedChild + 1];
							tempList.CurrentChosen = true;
							composite.temp = tempList.children;
							composite.tempSelected = 0;
						}
					}
				}
			}
		}

		public void returnComposite()
		{
			//OurLogger.Log(composite.selectedChild.ToString());
			if (composite.DepthCheck == true)
			{
				if (this.ChangeTimer.ElapsedTime.AsMilliseconds() > 200)
				{
					this.ChangeTimer.Restart();
					Button temp = (Button)composite.temp[composite.tempSelected];

					if (temp.CheckText() == "Back")
					{
						//OurLogger.Log(temp.CheckText());
						composite.DepthCheck = false;
						CompositeUI tempList = (CompositeUI)composite.children[composite.selectedChild + 1];
						tempList.CurrentChosen = false;
						composite.temp = null;
					}
				}
			}
		}
		
	}
}
