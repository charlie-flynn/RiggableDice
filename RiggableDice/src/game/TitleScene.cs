using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathLibrary;
using Raylib_cs;

namespace RiggableDice
{
    internal class TitleScene : Scene
    {
        public override void Start()
        {

            base.Start();
        }

        public override void Update(double deltaTime)
        {
            KeyboardKey keyPressed = (KeyboardKey)Raylib.GetKeyPressed();
            GoToDice(keyPressed);

            base.Update(deltaTime);
        }

        private void GoToDice(KeyboardKey key)
        {
            switch (key)
            {
                case KeyboardKey.One:
                    Game.CurrentScene = new DiceScene(1, 6);
                    break;
                case KeyboardKey.Two:
                    Game.CurrentScene = new DiceScene(2, 6);
                    break;
                case KeyboardKey.Three:
                    Game.CurrentScene = new DiceScene(3, 6);
                    break;
                case KeyboardKey.Four:
                    Game.CurrentScene = new DiceScene(4, 6);
                    break;
                case KeyboardKey.Five:
                    Game.CurrentScene = new DiceScene(5, 6);
                    break;
            }
        }
    }
}
