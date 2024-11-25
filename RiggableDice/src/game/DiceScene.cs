using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;
using MathLibrary;
using System.Security.Cryptography;

namespace RiggableDice
{
    internal class DiceScene : Scene
    {
        private Dice[] _dieArray;
        private int _diceBeingRigged;
        private int _riggedResultInput;
        private bool _isInputGreaterThanOneDigit;
        private bool _advantageRigModeIsOn;
        private Vector2 _center = new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2);
        private int _dieAmount;
        private int _dieMaxRoll;


        public DiceScene(int dieAmount, int dieMaxRoll)
        {
            _dieAmount = dieAmount;
            _dieMaxRoll = dieMaxRoll;
            _advantageRigModeIsOn = false;

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

            // check if the user inputted an F type of key to change the die being rigged
            // if the key was f1, enable advantage rig mode
            KeyboardKey keyPressed = (KeyboardKey)Raylib.GetKeyPressed();

            if (keyPressed == KeyboardKey.Backspace)
            {
                Game.CurrentScene = new TitleScene();
            }

            if (keyPressed >= (KeyboardKey)290 && keyPressed <= (KeyboardKey)300)
            {
                // if the key is F1, enter the Advantage Rig Mode
                if (keyPressed != (KeyboardKey)290)
                {
                    _diceBeingRigged = (int)keyPressed - 290;
                }
                else
                {
                    if (!_advantageRigModeIsOn)
                    {
                        _advantageRigModeIsOn = true;
                        _diceBeingRigged = 1;
                    }
                    else
                    {
                        _advantageRigModeIsOn = false;
                        _diceBeingRigged = 0;
                    }

                }


            }
            
            // let the player input numbers so they can rig the dice
            if ((keyPressed >= (KeyboardKey)48 && keyPressed <= (KeyboardKey)57) && _diceBeingRigged > 0 && _diceBeingRigged <= _dieArray.Length)
            {
                if (!_isInputGreaterThanOneDigit)
                {
                    _riggedResultInput = (int)keyPressed - 48;
                    _isInputGreaterThanOneDigit = true;
                }
                else
                {
                    _riggedResultInput = AppendDigit(_riggedResultInput, (int)keyPressed - 48);
                }

                Console.WriteLine(_riggedResultInput.ToString());
            }

            // if the player hits enter, rig the dice selected
            if (keyPressed == KeyboardKey.Enter && _diceBeingRigged > 0 && _diceBeingRigged <= _dieArray.Length)
            {
                // if advantage rig mode is enabled, rig all the dice to be lower than the rig value except for one, which will be the rig value
                // otherwise, the selected dice will have its result rigged to be the inputted result
                if (_advantageRigModeIsOn)
                {
                    int decidedDice = RandomNumberGenerator.GetInt32(0, _dieArray.Length);

                    for (int i = 0; i < _dieArray.Length; i++)
                    {
                        if (i == decidedDice)
                        {
                            _dieArray[i].RiggedResult = _riggedResultInput;
                        }
                        else
                        {
                            _dieArray[i].RiggedResult = RandomNumberGenerator.GetInt32(1, _riggedResultInput + 1);
                        }
                    }
                }
                else
                {
                    _dieArray[_diceBeingRigged - 1].RiggedResult = _riggedResultInput;
                }

                // reset all the rig values
                _advantageRigModeIsOn = false;
                _riggedResultInput = 0;
                _diceBeingRigged = 0;

            }

        }

        private int AppendDigit (int left, int right)
        {
            // convert the integers into strings to add the right to the left, then convert back into an int
            if (left != null && right != null)
                return int.Parse(left.ToString() + right.ToString());

            else
                return 0;
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
                    temp[0] = (Dice)Actor.Instantiate(new Dice(dieMaxRoll), null, new Vector2(_center.x - 65, _center.y));
                    temp[1] = (Dice)Actor.Instantiate(new Dice(dieMaxRoll), null, new Vector2(_center.x + 65, _center.y));
                    break;
                case 3:
                    temp[0] = (Dice)Actor.Instantiate(new Dice(dieMaxRoll), null, new Vector2(_center.x, _center.y));
                    temp[1] = (Dice)Actor.Instantiate(new Dice(dieMaxRoll), null, new Vector2(_center.x + 125, _center.y));
                    temp[2] = (Dice)Actor.Instantiate(new Dice(dieMaxRoll), null, new Vector2(_center.x - 125, _center.y));
                    break;
                case 4:
                    temp[0] = (Dice)Actor.Instantiate(new Dice(dieMaxRoll), null, new Vector2(_center.x - 65, _center.y - 65));
                    temp[1] = (Dice)Actor.Instantiate(new Dice(dieMaxRoll), null, new Vector2(_center.x + 65, _center.y - 65));
                    temp[2] = (Dice)Actor.Instantiate(new Dice(dieMaxRoll), null, new Vector2(_center.x - 65, _center.y + 65));
                    temp[3] = (Dice)Actor.Instantiate(new Dice(dieMaxRoll), null, new Vector2(_center.x + 65, _center.y + 65));
                    break;
                case 5:
                    temp[0] = (Dice)Actor.Instantiate(new Dice(dieMaxRoll), null, new Vector2(_center.x, _center.y));
                    temp[1] = (Dice)Actor.Instantiate(new Dice(dieMaxRoll), null, new Vector2(_center.x + 125, _center.y));
                    temp[2] = (Dice)Actor.Instantiate(new Dice(dieMaxRoll), null, new Vector2(_center.x - 125, _center.y));
                    temp[3] = (Dice)Actor.Instantiate(new Dice(dieMaxRoll), null, new Vector2(_center.x, _center.y - 125));
                    temp[4] = (Dice)Actor.Instantiate(new Dice(dieMaxRoll), null, new Vector2(_center.x, _center.y + 125));
                    break;
                case 6:
                    temp[0] = (Dice)Actor.Instantiate(new Dice(dieMaxRoll), null, new Vector2(_center.x, _center.y - 65));
                    temp[1] = (Dice)Actor.Instantiate(new Dice(dieMaxRoll), null, new Vector2(_center.x + 125, _center.y - 65));
                    temp[2] = (Dice)Actor.Instantiate(new Dice(dieMaxRoll), null, new Vector2(_center.x - 125, _center.y - 65));
                    temp[3] = (Dice)Actor.Instantiate(new Dice(dieMaxRoll), null, new Vector2(_center.x, _center.y + 65));
                    temp[4] = (Dice)Actor.Instantiate(new Dice(dieMaxRoll), null, new Vector2(_center.x + 125, _center.y + 65));
                    temp[5] = (Dice)Actor.Instantiate(new Dice(dieMaxRoll), null, new Vector2(_center.x - 125, _center.y + 65));
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
