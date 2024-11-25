using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Raylib_cs;
using MathLibrary;

namespace RiggableDice
{
    internal class Dice : Actor
    {
        private bool _isRolling = false;
        private float _fontSize;
        private double _timer;
        private int _lastDisplayedNumber;
        private int _displayedNumber;
        private int _maxRoll;
        private int _rollCount;
        private int _result;
        private int _riggedResult;
        public bool IsRolling { get => _isRolling; set => _isRolling = value; }

        public Dice(int maxRoll)
        {
            _maxRoll = maxRoll;
            _displayedNumber = maxRoll;
        }

        public override void Start()
        {
            base.Start();
            Transform.LocalScale = new Vector2(100, 100);
            _fontSize = Transform.LocalScale.x / 3;
        }

        public override void Update(double deltaTime)
        {
            base.Update(deltaTime);

            // if it is rolling, change the displayed number every half a second
            // and increment the roll count
            if (_isRolling)
            {
                if (_timer <= 0.0)
                {
                    if (_rollCount == 0)
                        _result = DetermineRoll();

                    _displayedNumber = RandomNumberGenerator.GetInt32(1, _maxRoll + 1);
                    if (_displayedNumber == _lastDisplayedNumber)
                    {
                        _displayedNumber++;
                        if (_displayedNumber > _maxRoll)
                            _displayedNumber -= 2;
                    }
                    _lastDisplayedNumber = _displayedNumber;
                    _rollCount++;
                    _timer = 0.1;
                }

                _timer -= deltaTime;
            }

            // when the roll count is equal to five or something, stop rolling and land on a number
            // if the dice was rigged, make it the specified value
            // if it was rigged advantage style, make it no higher than the specified value but make sure one dice has the specified value
            if (_rollCount == 10)
            {
                _isRolling = false;
                _rollCount = 0;
                _displayedNumber = _result;
            }

            // drawing

            Rectangle rectangle = new Rectangle(Transform.GlobalPosition, Transform.GlobalScale);

            Raylib.DrawRectanglePro(rectangle, Transform.GlobalScale / 2, 0, Color.Green);
            Raylib.DrawTextPro(new Font(), _displayedNumber.ToString(), new Vector2(_fontSize / 3, _fontSize / 2) * -1, Transform.GlobalPosition * -1, 0, _fontSize, 5, Color.White);

        }

        private int DetermineRoll()
        {
            int roll;

            if (_riggedResult > 0 && _riggedResult <= _maxRoll)
            {
                roll = _riggedResult;
            }
            else
            {
                roll = RandomNumberGenerator.GetInt32(1, _maxRoll + 1);
            }

            return roll;
        }
    }
}
