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
        Dice _dice;
        public override void Start()
        {

            base.Start();
            _dice = (Dice)Actor.Instantiate(new Dice(20), null, new Vector2(50, 50));
        }

        public override void Update(double deltaTime)
        {
            if (Raylib.IsKeyPressed(KeyboardKey.Space))
                _dice.IsRolling = true;

            base.Update(deltaTime);
        }
    }
}
