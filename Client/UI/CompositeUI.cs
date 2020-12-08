using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Collections.Generic;
using System;
using System.Text;
using Common.Utilities;

namespace Client.UI
{
	class CompositeUI : Component
	{
		public List<Component> children;
		public int selectedChild;
		public bool CurrentChosen = false;
		public Clock ChangeTimer { get; set; } = new Clock();

		public CompositeUI()
		{
			children = new List<Component>();
			selectedChild = -1;
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
			if (children[index].IsSelectable())
			{
				if (HasSelection())
				{
					children[selectedChild].Deselect();
				}

				children[index].Select();
				selectedChild = index;
				OurLogger.Log("Selecting child in composite: " + selectedChild.ToString());
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
				do
				{
					next = (next + 1) % children.Count;
				}
				while (!children[next].IsSelectable());

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
				do
				{
					prev = (prev + children.Count - 1) % children.Count;
				}
				while (!children[prev].IsSelectable());

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

	}
}
