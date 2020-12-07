using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Client.Utilities;
using Client.Config;

namespace Client.UI
{
	class Button : Component
	{
        private Action callback;
        private Texture normalTexture;
        private Texture selectedTexture;
        private Texture pressedTexture;
        private Sprite sprite;
        private Text text;
        private bool isToggle;

        public Button()
        {
            normalTexture = TextureHolder.GetInstance().Get(TextureIdentifier.ButtonDefault);
            selectedTexture = TextureHolder.GetInstance().Get(TextureIdentifier.ButtonSelected);
            pressedTexture = TextureHolder.GetInstance().Get(TextureIdentifier.ButtonClicked);
            sprite = new Sprite();
            text = new Text(string.Empty, ResourceHolderFacade.GetInstance().Fonts.Get(FontIdentifier.PixelatedSmall), 16);

            sprite.Texture = normalTexture;
            var bounds = sprite.GetLocalBounds();
            text.Position = new Vector2f(bounds.Width / 2, bounds.Height / 2);
        }

        public void SetCallback(Action callback)
        {
            this.callback = callback;
        }

        public void SetText(string text)
        {
            this.text.DisplayedString = text;
            var bounds = this.text.GetLocalBounds();
            this.text.Origin = new Vector2f(bounds.Width / 2, bounds.Height / 2);
        }

        public void SetToggle(bool flag)
        {
            isToggle = flag;
        }

        public override bool IsSelectable()
        {
            return true;
        }

        public override void Select()
        {
            base.Select();

            sprite.Texture = selectedTexture;
        }

        public override void Deselect()
        {
            base.Deselect();

            sprite.Texture = normalTexture;
        }

        public override void Activate()
        {
            base.Activate();

            // If we are toggle then we should show that the button is pressed and thus "toggled".
            if (isToggle)
            {
                sprite.Texture = pressedTexture;
            }

            if (callback != null)
            {
                callback();
            }

            // If we are not a toggle then deactivate the button since we are just momentarily activated.
            if (!isToggle)
            {
                Deactivate();
            }
        }

        public override void Deactivate()
        {
            base.Deactivate();

            if (isToggle)
            {
                // Reset texture to right one depending on if we are selected or not.
                if (IsSelected())
                {
                    sprite.Texture = selectedTexture;
                }
                else
                {
                    sprite.Texture = normalTexture;
                }
            }
        }

        //public override void HandleKeyboardEvent(Keyboard.Key key, bool isPressed)
        //{

        //}

        public override void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            target.Draw(sprite, states);
            target.Draw(text, states);
        }
    }
}
