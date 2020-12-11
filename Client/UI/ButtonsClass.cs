using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Common.Utilities;
using Client.Managers;

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
			playButton.SetText("Resume");

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
			CompositeUI leaf2 = new CompositeUI();
			CompositeUI leaf = new CompositeUI();

			var rightControlButton = new Button();
			rightControlButton.Position = new Vector2f(320, 250);
			rightControlButton.SetText("WASD");

			var arrowControlButton = new Button();
			arrowControlButton.Position = new Vector2f(420, 250);
			arrowControlButton.SetText("Arrows");

			var volumeLowerButton = new Button();
			volumeLowerButton.Position = new Vector2f(290, 300);
			volumeLowerButton.SetText("<");

			var volumeHigherButton = new Button();
			volumeHigherButton.Position = new Vector2f(450, 300);
			volumeHigherButton.SetText(">");

			var volumeButton = new Button();
			volumeButton.Position = new Vector2f(370, 300);
			volumeButton.SetText("50");

			var backButton = new Button();
			backButton.Position = new Vector2f(370, 350);
			backButton.SetText("Back");

			var smallCursorButton = new Button();
			smallCursorButton.Position = new Vector2f(290, 200);
			smallCursorButton.SetText("Small");

			var mediumCursorButton = new Button();
			mediumCursorButton.Position = new Vector2f(370, 200);
			mediumCursorButton.SetText("Medium");

			var bigCursorButton = new Button();
			bigCursorButton.Position = new Vector2f(450, 200);
			bigCursorButton.SetText("Big");

			var cursorButton = new Button();
			cursorButton.Position = new Vector2f(370, 150);
			cursorButton.SetText("Settings");

			leaf.Add(smallCursorButton);
			leaf.Add(mediumCursorButton);
			leaf.Add(bigCursorButton);
			leaf.Add(rightControlButton);
			leaf.Add(arrowControlButton);
			leaf.Add(volumeLowerButton);
			leaf2.Add(volumeButton);
			leaf2.Add(cursorButton);
			leaf.Add(volumeHigherButton);
			leaf.Add(backButton);
			leaf2.CurrentChosen = true;
			leaf.Add(leaf2);
			composite.Add(leaf);
			
			//-------------------------------
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
		public void selectButton()
		{
				if(composite.HasSelection())
				{
					Button tempButton = (Button)composite.children[composite.selectedChild];
					if(tempButton.CheckText() == "Resume")
					{
						ChangeVisibility();
					}
					else if(tempButton.CheckText() == "Exit")
					{
						GameState.GetInstance().CloseWindow = true;
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
					this.composite.ChangeTimer.Restart();
					if (composite.HasSelection())
					{
						composite.children[composite.selectedChild].Activate();
						Button tempButton = (Button)composite.children[composite.selectedChild];
						if (tempButton.CheckToggle())
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
