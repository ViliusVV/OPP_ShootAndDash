using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Collections.Generic;
using System;
using System.Text;
using Common.Utilities;
using Client.UI.Visitor;

namespace Client.UI
{
	class CompositeUI : Component
	{
		public List<Component> children;
		public List<Component> temp;
		public int selectedChild;
		public int tempSelected;
		public bool CurrentChosen = false;
		public bool DepthCheck = false;
		public Clock ChangeTimer { get; set; } = new Clock();

		public CompositeUI()
		{
			children = new List<Component>();
			temp = new List<Component>();
			temp = children;
			selectedChild = -1;
			tempSelected = 0;
		}

		public void Add(Component component)
		{
			children.Add(component);

			if (!HasSelection() && component.IsSelectable())
			{
				Select(children.Count - 1);
			}
		}

		public void Remove(Component component)
		{
			children.Remove(component);
		}

		public override bool IsSelectable()
		{
			return false;
		}

		//public override void HandleKeyboardEvent(Keyboard.Key key, bool isPressed)
		//{
		//	// If we have selected a child then give it events
		//	if (HasSelection() && children[selectedChild].IsActive())
		//	{
		//		children[selectedChild].HandleKeyboardEvent(key, isPressed);
		//	}
		//	else if (!isPressed)
		//	{
		//		if (key == Keyboard.Key.W || key == Keyboard.Key.Up)
		//		{
		//			SelectPrevious();
		//		}
		//		else if (key == Keyboard.Key.S || key == Keyboard.Key.Down)
		//		{
		//			SelectNext();
		//		}
		//		else if (key == Keyboard.Key.Return || key == Keyboard.Key.Space)
		//		{
		//			if (HasSelection())
		//			{
		//				children[selectedChild].Activate();
		//			}
		//		}
		//	}
		//}

		public bool HasSelection()
		{
			return selectedChild >= 0;
		}

		public void Select(int index)
		{
			if (DepthCheck == false)
			{
				if (children[index].IsSelectable())
				{
					if (HasSelection())
					{
						children[selectedChild].Deselect();
					}

					children[index].Select();
					selectedChild = index;
					//OurLogger.Log("Selecting child in composite: " + selectedChild.ToString());
				}
			}
			else
			{
				if (temp[index].IsSelectable())
				{
					if (HasSelection())
					{
						temp[tempSelected].Deselect();
					}

					temp[index].Select();
					tempSelected = index;
					//OurLogger.Log("Selecting child in composite: " + tempSelected.ToString());
				}
			}
		}

		public void SelectNext()
		{
			if (this.ChangeTimer.ElapsedTime.AsMilliseconds() > 200)
			{
				this.ChangeTimer.Restart();
				if (!HasSelection()) return;

				// Search next component that is selectable, wrap around if necessary
				int next = selectedChild;
				if (DepthCheck == false)
				{
					do
					{
						next = (next + 1) % children.Count;
					}
					while (!children[next].IsSelectable());
				}
				else
				{
					next = tempSelected;
					do
					{
						next = (next + 1) % temp.Count;
					}
					while (!temp[next].IsSelectable());
				}

				// Select that component
				Select(next);
			}
		}

		public void SelectPrevious()
		{
			if (this.ChangeTimer.ElapsedTime.AsMilliseconds() > 200)
			{
				this.ChangeTimer.Restart();
				if (!HasSelection()) return;

				// Search previous component that is selectable, wrap around if necessary
				int prev = selectedChild;
				if (DepthCheck == false)
				{
					do
					{
						prev = (prev + children.Count - 1) % children.Count;
					}
					while (!children[prev].IsSelectable());
				}
				else
				{
					prev = tempSelected;
					do
					{
						prev = (prev + temp.Count - 1) % temp.Count;
					}
					while (!temp[prev].IsSelectable());
				}

				// Select that component
				Select(prev);
			}
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			states.Transform *= Transform;

			foreach(var child in children)
			{
				if (this.CurrentChosen == true)
				{
					target.Draw(child, states);
				}
			}
		}

		public void Accept(IVisitor visitor)
		{
			if (this.ChangeTimer.ElapsedTime.AsMilliseconds() > 200)
			{
				this.ChangeTimer.Restart();
				Button textCheck = (Button)children[selectedChild];
				if (textCheck.CheckText() == "Settings")
				{
					OurLogger.Log("Testing visit");
					visitor.Visit(temp[tempSelected]);
				}
			}
		}

	}
}
