using Client.UI.Interpreter;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.UI
{
    class InGameLog : Drawable
    {
        public bool ShowLog { get; set; } = false;
        private List<Tuple<CustomText, long>> _lines = new List<Tuple<CustomText, long>>();

        private CustomText _currentLine = new CustomText(14);

        private float _startY = 600f;
        private float _deltaY = 16f;

        public InGameLog()
        {
            _currentLine.DisplayedString = ">> ";
            _currentLine.Position = new Vector2f(10f, 600f);
        }


        public void Draw(RenderTarget target, RenderStates states)
        {
            if (this.ShowLog)
            {
                target.Draw(_currentLine);
            }

            for(int i = _lines.Count - 1; i >= 0; i--)
            {
                long delta = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - _lines[i].Item2;
                Vector2f pos = new Vector2f(0f, _startY - (_deltaY * (i + 1)));
                _lines[i].Item1.Position = pos;

                if(this.ShowLog || delta < 2000)
                target.Draw(_lines[i].Item1);
            }

        }

        public void AddText(String text)
        {
            _currentLine.DisplayedString = _currentLine.DisplayedString + text.Trim('`');
        }

        public void ClearLine()
        {
            _currentLine.DisplayedString = ">> ";
        }

        public void ConfirmLine()
        {
            string tmpStr = _currentLine.DisplayedString.Remove(0, 3).Trim();


            _lines.Insert(0, new Tuple<CustomText, long>(
                new CustomText(14) { DisplayedString = tmpStr },
                DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
             );

            _currentLine.DisplayedString = ">> ";


            Context context = new Context(tmpStr);



            // Build the 'parse tree'

            List<IExpression> tree = new List<IExpression>();

            tree.Add(new HealExpression());

            tree.Add(new FireRateExpression());

            tree.Add(new ThousandExpression());

            tree.Add(new HundredExpression());

            tree.Add(new TenExpression());

            tree.Add(new OneExpression());



            // Interpret

            foreach (IExpression exp in tree)

            {

                exp.Interpret(context);

            }

            if(context.ActionName != null)
            {
                _lines.Insert(0, new Tuple<CustomText, long>(
                    new CustomText(14) { DisplayedString = $"==>> {context.ActionName} {context.OutputNumber}" },
                    DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
                 ); ;

                if (context.ActionName.Equals("heal"))
                {
                    GameApplication.GetInstance().MainPlayer.AddHealth(context.OutputNumber);
                }

                if (context.ActionName.Equals("speed up"))
                {
                    GameApplication.GetInstance().MainPlayer.Weapon.AttackSpeed = context.OutputNumber;
                }
            }
            else
            {
                _lines.Insert(0, new Tuple<CustomText, long>(
                    new CustomText(14) { DisplayedString = $"Unknown command/action" },
                    DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
                 ); ;
            }

        }
    }
}
