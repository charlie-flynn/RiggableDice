using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;
using MathLibrary;

namespace RiggableDice
{
    internal class DiceScene : Scene
    {
        private Dice[] _dieArray;
        private Vector2 _center = new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2);
        private int _dieAmount;
        private int _dieMaxRoll;

        public DiceScene(int dieAmount, int dieMaxRoll)
        {
            _dieAmount = dieAmount;
            _dieMaxRoll = dieMaxRoll;


        }

        public override void Start()
        {
            base.Start();
            _dieArray = InstantiateDie(_dieAmount, _dieMaxRoll);
        }

        public override void Update(double deltaTime)
        {
            base.Update(deltaTime);

            // check if the player pressed space to roll all of the die
            if (Raylib.IsKeyPressed(KeyboardKey.Space))
            {
                foreach (Dice dice in _dieArray)
                {
                    dice.IsRolling = true;
                }
            }

            // check if the user inputted an F type of key to determine whether the user is allowed to input a rigged roll or not
        }

        private Dice[] InstantiateDie(int dieAmount, int dieMaxRoll)
        {
            Dice[] temp = new Dice[dieAmount];

            // switch case for up to nine die

            switch (dieAmount)
            {
                case 1:
                    temp[0] = (Dice)Actor.Instantiate(new Dice(dieMaxRoll), null, _center);
                    break;
                case 2:
                    temp[0] = (Dice)Actor.Instantiate(new Dice(dieMaxRoll), null, new Vector2(_center.x - 75, _center.y));
                    temp[1] = (Dice)Actor.Instantiate(new Dice(dieMaxRoll), null, new Vector2(_center.x + 75, _center.y));
                    break;
                case 3:
                    temp[0] = (Dice)Actor.Instantiate(new Dice(dieMaxRoll), null, new Vector2(_center.x, _center.y));
                    temp[1] = (Dice)Actor.Instantiate(new Dice(dieMaxRoll), null, new Vector2(_center.x + 125, _center.y));
                    temp[2] = (Dice)Actor.Instantiate(new Dice(dieMaxRoll), null, new Vector2(_center.x - 125, _center.y));
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 7:
                    break;
                case 8:
                    break;
                case 9:
                    break;
            }

            return temp;
        }
    }
}
